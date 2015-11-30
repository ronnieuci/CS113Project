using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ShapesArray : MonoBehaviour
{

    public GameObject[,] shapes = new GameObject[Constants.Rows, Constants.Columns];		//Array used to map or manage blocks
	private GameObject NullBlock;															//Instance of Nullblock, used for swapping blocks to an empty space
    
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {	return shapes[row, column];		}
            catch (Exception ex)
            {	throw;	}
        }
        set
        {	shapes[row, column] = value;	}
    }

	public void setNullBlock(GameObject block)
	{
		NullBlock = block;
	}

	/// Swaps the position of two items, also keeping a backup
	public void Swap(GameObject g1, GameObject g2)
	{
			var g1Shape = g1.GetComponent<Shape> ();
			var g2Shape = g2.GetComponent<Shape> ();
			
			//swap them in the array
			var temp = shapes [g1Shape.Row, g1Shape.Column];
			shapes [g1Shape.Row, g1Shape.Column] = shapes [g2Shape.Row, g2Shape.Column];
			shapes [g2Shape.Row, g2Shape.Column] = temp;
			
			//swap their respective properties
			Shape.SwapColumnRow (g1Shape, g2Shape);
	}

    /// Returns the matches found for a list of GameObjects
    /// MatchesInfo class is not used as this method is called on subsequent collapses/checks, 
    /// not the one inflicted by user's drag
    public IEnumerable<GameObject> GetMatches(IEnumerable<GameObject> gos)
    {
        List<GameObject> matches = new List<GameObject>();
        foreach (var go in gos)
        {	matches.AddRange(GetMatches(go).MatchedBlock);	}
        return matches.Distinct();
    }
	
    /// Returns the matches found for a single GameObject
    public MatchesInfo GetMatches(GameObject go) {
        MatchesInfo matchesInfo = new MatchesInfo();

        var horizontalMatches = GetMatchesHorizontally(go);
        if (ContainsDestroyRowColumnBonus(horizontalMatches)) {
            horizontalMatches = GetEntireRow(go);
            if (!BonusTypeUtilities.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
                matchesInfo.BonusesContained |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(horizontalMatches);

        var verticalMatches = GetMatchesVertically(go);
        if (ContainsDestroyRowColumnBonus(verticalMatches)) {
            verticalMatches = GetEntireColumn(go);
            if (!BonusTypeUtilities.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
                matchesInfo.BonusesContained |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(verticalMatches);

        return matchesInfo;
    }
	
    private bool ContainsDestroyRowColumnBonus(IEnumerable<GameObject> matches) {
        if (matches.Count() >= Constants.MinimumMatches) {
            foreach (var go in matches) {
                if (BonusTypeUtilities.ContainsDestroyWholeRowColumn
                    (go.GetComponent<Shape>().Bonus))
                    return true;
            }
        }
        return false;
    }

    private IEnumerable<GameObject> GetEntireRow(GameObject go) {
        List<GameObject> matches = new List<GameObject>();
        int row = go.GetComponent<Shape>().Row;
        for (int column = 0; column < Constants.Columns; column++) {
            matches.Add(shapes[row, column]);
        }
        return matches;
    }

    private IEnumerable<GameObject> GetEntireColumn(GameObject go) {
        List<GameObject> matches = new List<GameObject>();
        int column = go.GetComponent<Shape>().Column;
        for (int row = 0; row < Constants.Rows; row++) {
            matches.Add(shapes[row, column]);
        }
        return matches;
    }
	
    /// Searches horizontally for matches
    private IEnumerable<GameObject> GetMatchesHorizontally(GameObject go) {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var shape = go.GetComponent<Shape>();
        //check left
        if (shape.Column != 0) {
			for (int column = shape.Column - 1; column >= 0; column--) {
				if (shapes [shape.Row, column] == null) {
					break;
				} else if (shapes [shape.Row, column].GetComponent<Shape> ().IsSameType (shape)) {
					matches.Add (shapes [shape.Row, column]);
				} else {
					break;
				}
			}
		}

        //check right
        if (shape.Column != Constants.Columns - 1)
			for (int column = shape.Column + 1; column < Constants.Columns; column++) {
				if (shapes [shape.Row, column] == null) {
					break;
				} else if (shapes [shape.Row, column].GetComponent<Shape> ().IsSameType (shape)) {
					matches.Add (shapes [shape.Row, column]);
				} else
					break;
			}

        //we want more than three matches
        if (matches.Count < Constants.MinimumMatches)
            matches.Clear();

        return matches.Distinct();
    }
	
    /// Searches vertically for matches
    private IEnumerable<GameObject> GetMatchesVertically(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var shape = go.GetComponent<Shape>();
        //check bottom
        if (shape.Row != 0)
			for (int row = shape.Row - 1; row >= 0; row--) {
				if (shapes [row, shape.Column] != null &&
					shapes [row, shape.Column].GetComponent<Shape> ().IsSameType (shape)) {
					matches.Add (shapes [row, shape.Column]);
				} else
					break;
			}

        //check top
        if (shape.Row != Constants.Rows - 1)
			for (int row = shape.Row + 1; row < Constants.Rows; row++) {
				if (shapes [row, shape.Column] != null && 
					shapes [row, shape.Column].GetComponent<Shape> ().IsSameType (shape)) {
					matches.Add (shapes [row, shape.Column]);
				} else
					break;
			}

        if (matches.Count < Constants.MinimumMatches)
            matches.Clear();

        return matches.Distinct();
    }

    /// Removes (sets as null) an item from the array
    public void Remove(GameObject item)
    {
		if (item != null) {
			shapes [item.GetComponent<Shape> ().Row, item.GetComponent<Shape> ().Column] = null;
		}
	}

    /// Collapses the array on the specific columns, after checking for empty items on them
    public AlteredBlockInfo Collapse(IEnumerable<int> columns)
    {
        AlteredBlockInfo collapseInfo = new AlteredBlockInfo();


        ///search in every column
        foreach (var column in columns) {
			//begin from bottom row
			for (int row = 0; row < Constants.Rows; row++) {
				//if you find a null item
				if (shapes [row, column] == null) {
					//start searching for the first non-null
					for (int row2 = row + 1; row2 < Constants.Rows; row2++) {
						//if you find one, bring it down (i.e. replace it with the null you found)
						if (shapes [row2, column] != null) {
							shapes [row, column] = shapes [row2, column];
							shapes [row2, column] = null;

							//calculate the biggest distance
							if (row2 - row > collapseInfo.MaxDistance) 
								collapseInfo.MaxDistance = row2 - row;

							//assign new row and column (name does not change)
							shapes [row, column].GetComponent<Shape> ().Row = row;
							shapes [row, column].GetComponent<Shape> ().Column = column;

							collapseInfo.AddBlock (shapes [row, column]);
							break;
						}
					}
				} else if (shapes [row, column] == NullBlock) {
					//start searching for the first non-null
					for (int row2 = row + 1; row2 < Constants.Rows; row2++) {
						//if you find one, bring it down (i.e. replace it with the null you found)
						if (shapes [row2, column] != null) {
							shapes [row, column] = shapes [row2, column];
							shapes [row2, column] = null;
							
							//calculate the biggest distance
							if (row2 - row > collapseInfo.MaxDistance) 
								collapseInfo.MaxDistance = row2 - row;
							
							//assign new row and column (name does not change)
							shapes [row, column].GetComponent<Shape> ().Row = row;
							shapes [row, column].GetComponent<Shape> ().Column = column;
							
							collapseInfo.AddBlock (shapes [row, column]);
							break;
						}
					}
				}
			}
		}
        return collapseInfo;
    }
	
    /// Searches the specific column and returns info about null items
    public IEnumerable<ShapeInfo> GetEmptyItemsOnColumn(int column)
    {
		List<ShapeInfo> emptyItems = new List<ShapeInfo> ();
		for (int row = 0; row < Constants.Rows; row++) {
			if (shapes [row, column] == null)
				emptyItems.Add (new ShapeInfo () { Row = row, Column = column });
		}
		return emptyItems;
	}
	
	public IEnumerable<GameObject> getAllBlocksOfColor(GameObject k){
		GameObject[] temp = new GameObject[blockCountofColor(k)];
		int count = 0;
		for (int col = 0; col < Constants.Columns; col++) {
			for (int row = 0; row < Constants.Rows; row++) {
				if (shapes[row,col].GetComponent<Shape>() != null && this[row,col].GetComponent<Shape>().IsSameType(k.GetComponent<Shape>())){
					temp[count] = shapes[row,col];
					count++;
				}
			}
		}
		return temp;	
	}

	public IEnumerable<GameObject> GetAllBlocks(){
		
		GameObject[] temp = new GameObject[blockCount()];
		int count = 0;
		for (int col = 0; col < Constants.Columns; col++) {
			for (int row = 0; row < Constants.Rows; row++) {
				if (shapes[row,col].GetComponent<Shape>() != null){
					temp[count] = shapes[row,col];
					count++;
				}
			}
		}
		return temp;
	}

	private int blockCountofColor(GameObject k) {
		int count = 0;
		for (int col = 0; col < Constants.Columns; col++) {
			for (int row = 0; row < Constants.Rows; row++) {
				if (this[row,col] != null && this[row,col].GetComponent<Shape>().IsSameType(k.GetComponent<Shape>())){
					count++;
				}
			}
		}
		return count;
	}

	private int blockCount() {
		int count = 0;
		for (int col = 0; col < Constants.Columns; col++) {
			for (int row = 0; row < Constants.Rows; row++) {
				if (this[row,col] != null){
					count++;
				}
			}
		}
		return count;
	}

	public IEnumerable<GameObject> getRandomBlocks(int count) {
		System.Random RNG = new System.Random ();
		var blockSet = new GameObject[count];
		for (int c = 0; c < count; c++) {
			var x = RNG.Next (0,Constants.Rows-1);
			var y = RNG.Next (0,Constants.Columns-1);

			while (shapes[x,y] == null)
			{
				x = RNG.Next (0,Constants.Rows-1);
				y = RNG.Next (0,Constants.Columns-1);
			}
			blockSet[c] = shapes[x,y];
		}
		return blockSet;
	}

	//STILL IN PROGRESS//
	//Moving blocks in the array to match CW-rotation
	public void RotateCW(){
		GameObject[,] temp = new GameObject[Constants.Rows,Constants.Columns];
		int rTemp = 0;

		for (int col = 0; col < Constants.Columns; col++) {
			int cTemp = Constants.Rows-1;
			for (int row = 0; row < Constants.Rows; row++) {
				if(this.shapes[rTemp,cTemp] != null){
					temp[row,col] = this.shapes[rTemp,cTemp];
					temp[row,col].GetComponent<Shape>().Column = col;
					temp[row,col].GetComponent<Shape>().Row = row;
				}
				cTemp--;
			}
			rTemp++;
		}
		this.shapes = temp;
	}
	
	public void RotateCCW(){
		GameObject[,] temp = new GameObject[Constants.Rows,Constants.Columns];
		int rTemp = 0;
		
		for (int col = 0; col < Constants.Columns; col++) {
			int cTemp = Constants.Rows-1;
			for (int row = 0; row < Constants.Rows; row++) {
				if(this.shapes[row,col] != null){
					temp[rTemp,cTemp] = this.shapes[row,col];
					temp[rTemp,cTemp].GetComponent<Shape>().Column = cTemp;
					temp[rTemp,cTemp].GetComponent<Shape>().Row = rTemp;
				}
				cTemp--;
			}
			rTemp++;
		}
		this.shapes = temp;
	}
}