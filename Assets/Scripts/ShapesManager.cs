using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShapesManager : MonoBehaviour
{
	public Text ScoreText; 
	public ShapesArray shapes;
	public PlayerInput play;
	public Transform parent;
	public Vector2 middlePoint;
	public Vector2 BlockSize = new Vector2 (0.2f, 0.2f);
	private Vector2 swapDirection1, swapDirection2;
	public int score;
	public int[] ch = new int[3];
	private float xDiv,yDiv;
	private GameState state = GameState.None;
	private GameObject hitGo = null;
	private Vector2[] SpawnPositions;
	public GameObject NullBlock;
	public GameObject[] BlockPrefabs, ExplosionPrefabs, BonusPrefabs;
	private IEnumerator CheckPotentialMatchesCoroutine;
	public Characters[] playerChar = new Characters[3];

	void Awake ()
	{
		swapDirection1 = Vector2.right;
		swapDirection2 = Vector2.left;
		xDiv = -3.5f; yDiv = -11.5f;
	}

	void Start ()
	{
		ch [0] = 1;
		ch [1] = 2;
		ch [2] = 3;

		InitializeTypesOnPrefabShapesAndBonuses ();
		InitializeVariables ();
		InitializeBlockAndSpawnPositions ();
		setCharacters (ch);
	}


	void setCharacters(int[] c)
	{
		int m = 0;
		foreach(int i in c)
		{
			playerChar[m].setChar(i,m);
			m+=1;
		}
	}

	// Update is called once per frame
	void FixedUpdate (){
		if (state == GameState.None) {
			if (Input.GetKey (play.swap)) {
				//get the hit position
				var hit = Physics2D.Raycast (play.getCursorLocation (), swapDirection1, 1.0f);
				if (hit.collider != null) {
					hitGo = hit.collider.gameObject;
					state = GameState.SelectionStarted;
				} else {
					InstantiateAndPlaceNewBlock (play.y,(play.x + 1), NullBlock);
					hit = Physics2D.Raycast (play.getCursorLocation (), swapDirection1, 1.0f);
					hitGo = hit.collider.gameObject;
					state = GameState.SelectionStarted;
				}
			} 
		} else if (state == GameState.SelectionStarted) {
			var hit = Physics2D.Raycast (play.getCursorLocation (), swapDirection2, 1.0f);

			//we have a hit
			if(hit.collider == null)
			{
				if (hitGo.GetComponent<Shape> ().Type == NullBlock.name)
				{
					Destroy (hitGo);
					state = GameState.None;
				} else {
					InstantiateAndPlaceNewBlock (play.y,play.x, NullBlock);
					hit = Physics2D.Raycast (play.getCursorLocation (), swapDirection2, 1.0f);
					state = GameState.Animating;
					FixSortingLayer (hitGo, hit.collider.gameObject);
					StartCoroutine (FindMatches (hit));
				}
			}
			else if (hit.collider != null && hitGo != hit.collider.gameObject) {
				//if the two shapes are diagonally aligned (different row and column), just return
				if (!Utilities.AreVerticalOrHorizontalNeighbors (hitGo.GetComponent<Shape> (), hit.collider.gameObject.GetComponent<Shape> ())) 
				{
					state = GameState.None;
				} else {
					state = GameState.Animating;
					FixSortingLayer (hitGo, hit.collider.gameObject);
					StartCoroutine (FindMatches (hit));
				}
			}
		}
	}
	
	/// Initialize shapes
	private void InitializeTypesOnPrefabShapesAndBonuses ()
	{
		//just assign the name of the prefab
		foreach (var item in BlockPrefabs) {
			item.GetComponent<Shape> ().Type = item.name;
		}

		NullBlock.GetComponent<Shape> ().Type = NullBlock.name;
		//assign the name of the respective "normal" as the type of the Bonus
		foreach (var item in BonusPrefabs) {
			item.GetComponent<Shape> ().Type = BlockPrefabs.Where (x => x.GetComponent<Shape> ().Type.Contains (item.name.Split ('_') [1].Trim ())).Single ().name;
		}
	}

	private void InitializeVariables ()
	{
		score = 0;
		ShowScore();
	}

	public void InitializeBlockAndSpawnPositions ()
	{	
		if (shapes != null)
			ResetBoard ();
		shapes = new ShapesArray ();
		SpawnPositions = new Vector2[Constants.Columns];

		for (int row = 0; row < Constants.Rows; row++) 
		{
			for (int column = 0; column < Constants.Columns; column++) 
			{
				GameObject newBlock = GetRandomBlock ();
				
				//check if two previous horizontal are of the same type
				while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>()) 
				       			   && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())) 
				{
					newBlock = GetRandomBlock ();
				}
				
				//check if two previous vertical are of the same type
				while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())
				       			&& shapes[row - 2, column].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())) 
				{
					newBlock = GetRandomBlock ();
				}
				InstantiateAndPlaceNewBlock (row, column, newBlock);
			}
		}
		SetupSpawnPositions ();
	}

	private void InstantiateAndPlaceNewBlock (int row, int column, GameObject newBlock)
	{
		GameObject go = Instantiate (newBlock, middlePoint + new Vector2 (xDiv +(column * BlockSize.x) + parent.transform.position.x , yDiv + Constants.Columns + (BlockSize.y * row)), Quaternion.identity) as GameObject;

		//assign the specific properties
		go.GetComponent<Shape> ().Assign (newBlock.GetComponent<Shape> ().Type, row, column);
		shapes [row, column] = go;
		go.transform.parent = parent;
	}
	
	private void SetupSpawnPositions ()
	{
		//create the spawn positions for the new shapes (will pop from the 'ceiling')
		for (int column = 0; column < Constants.Columns; column++) {
			SpawnPositions [column] = middlePoint + new Vector2 ((column * BlockSize.x) + parent.transform.position.x, (Constants.Rows * BlockSize.y));
		}
	}

	/// Destroy all Block gameobjects and resets board
	public void ResetBoard ()
	{
		for (int row = 0; row < Constants.Rows; row++) {
			for (int column = 0; column < Constants.Columns; column++) 
			{
				if (shapes[row,column] != null)
					RemoveFromScene(shapes [row, column]);
			}
		}
		InitializeBlockAndSpawnPositions ();
	}
	
	/// Modifies sorting layers for better appearance when dragging/animating
	private void FixSortingLayer (GameObject hitGo, GameObject hitGo2)
	{
		SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer> ();
		SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer> ();
		if (sp1.sortingOrder <= sp2.sortingOrder) {
			sp1.sortingOrder = 1;
			sp2.sortingOrder = 0;
		}
	}

	private IEnumerator FindMatches (RaycastHit2D hit2)
	{
		//get the second item that was part of the swipe
		var hitGo2 = hit2.collider.gameObject;
		shapes.Swap (hitGo, hitGo2);
		
		//move the swapped ones
		hitGo.transform.positionTo (Constants.AnimationDuration, hitGo2.transform.position);
		hitGo2.transform.positionTo (Constants.AnimationDuration, hitGo.transform.position);
		yield return new WaitForSeconds (Constants.AnimationDuration);

		List<int> columns = new List<int>();

		columns.Add((int)(hitGo.transform.position.x+8.5f));
		columns.Add((int)(hitGo2.transform.position.x+8.5f));

		//get the matches via the helper methods
		var hitGomatchesInfo = shapes.GetMatches (hitGo);
		var hitGo2matchesInfo = shapes.GetMatches (hitGo2);
		var totalMatches = hitGomatchesInfo.MatchedBlock.Union (hitGo2matchesInfo.MatchedBlock).Distinct ();

		//if more than 3 matches and no Bonus is contained in the line, we will award a new Bonus
		bool addBonus = totalMatches.Count () >= Constants.MinimumMatchesForBonus &&
			!BonusTypeUtilities.ContainsDestroyWholeRowColumn (hitGomatchesInfo.BonusesContained) &&
			!BonusTypeUtilities.ContainsDestroyWholeRowColumn (hitGo2matchesInfo.BonusesContained);
		
		Shape hitGoCache = null;
		if (addBonus) {
			hitGoCache = new Shape ();
			//get the game object that was of the same type
			var sameTypeGo = hitGomatchesInfo.MatchedBlock.Count () > 0 ? hitGo : hitGo2;
			var shape = sameTypeGo.GetComponent<Shape> ();
			//cache it
			hitGoCache.Assign (shape.Type, shape.Row, shape.Column);
		}

		if (hitGo.GetComponent<Shape> ().Type == NullBlock.name) {
			shapes.setNullBlock (hitGo);
			Destroy (hitGo);
		} else if (hitGo2.GetComponent<Shape> ().Type == NullBlock.name) {
			shapes.setNullBlock (hitGo2);
			Destroy (hitGo2);
		}

		int timesRun = 1;
	RESTART:
		if (totalMatches.Count () > 0) {
			while (totalMatches.Count() >= Constants.MinimumMatches) {
				//increase score
				IncreaseScore ((totalMatches.Count () - 2) * Constants.Match3Score);
			
				if (timesRun >= 2)
					IncreaseScore (Constants.SubsequentMatchScore);

				foreach (var item in totalMatches) {
					if (item != null) {
						//get the columns that we had a collapse
						columns.Add (item.GetComponent<Shape> ().Column);
						shapes.Remove (item);
						RemoveFromScene (item);
					}
				}
			
				//check and instantiate Bonus if needed
				if (addBonus)
					CreateBonus (hitGoCache);
					addBonus = false;

				//the order the 2 methods below get called is important!!!
				//collapse the ones gone
				var collapsedBlockInfo = shapes.Collapse (columns);
				int maxDistance = Mathf.Max (collapsedBlockInfo.MaxDistance);
			
				MoveAndAnimate (collapsedBlockInfo.AlteredBlock, maxDistance);
				//will wait for both of the above animations
				yield return new WaitForSeconds (Constants.MoveAnimationMinDuration);
			
				//search if there are matches with the new/collapsed items
				totalMatches = shapes.GetMatches (collapsedBlockInfo.AlteredBlock);
				timesRun++;

				collapsedBlockInfo = shapes.Collapse (columns);
				maxDistance = Mathf.Max (collapsedBlockInfo.MaxDistance);
			}
		} else {
			var collapsedBlockInfo = shapes.Collapse (columns);
			int maxDistance = Mathf.Max (collapsedBlockInfo.MaxDistance);
			if (maxDistance < 1)
				maxDistance = 1;
			while (collapsedBlockInfo.MaxDistance > 0 )
			{
			
				MoveAndAnimate (collapsedBlockInfo.AlteredBlock, maxDistance);
				//will wait for both of the above animations
				yield return new WaitForSeconds (Constants.MoveAnimationMinDuration);
			
				//search if there are matches with the new/collapsed items
				totalMatches = shapes.GetMatches (collapsedBlockInfo.AlteredBlock);

				collapsedBlockInfo = shapes.Collapse (columns);
				maxDistance = Mathf.Max (collapsedBlockInfo.MaxDistance);
				if (totalMatches.Count() > 0)
					goto RESTART;	
			}
		}
		state = GameState.None;
	}

	/// Creates a new Bonus based on the shape parameter
	private void CreateBonus (Shape hitGoCache)
	{
		GameObject Bonus = Instantiate (GetBonusFromType (hitGoCache.Type), middlePoint
			+ new Vector2 (xDiv + (hitGoCache.Column * BlockSize.x) + parent.transform.position.x,
		              	  xDiv + (hitGoCache.Row * BlockSize.y) + parent.transform.position.y), Quaternion.identity)
			as GameObject;
		Bonus.transform.parent = parent;
		shapes [hitGoCache.Row, hitGoCache.Column] = Bonus;
		var BonusShape = Bonus.GetComponent<Shape> ();
		//will have the same type as the "normal" candy
		BonusShape.Assign (hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
		//add the proper Bonus type
		BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;
	}

	/// <summary>
	/// Animates gameobjects to their new position
	/// </summary>
	/// <param name="movedGameObjects"></param>
	private void MoveAndAnimate (IEnumerable<GameObject> movedGameObjects, int distance)
	{
		foreach (var item in movedGameObjects) {
			item.transform.positionTo (Constants.MoveAnimationMinDuration * distance, middlePoint + new Vector2 ((xDiv + (item.GetComponent<Shape> ().Column * BlockSize.x) + parent.transform.position.x),(xDiv +(item.GetComponent<Shape> ().Row * BlockSize.y)) + parent.transform.position.y));
		}
	}

	private void RemoveFromScene (GameObject item)
	{
		GameObject explosion = GetRandomExplosion ();
		var newExplosion = Instantiate (explosion, item.transform.position, Quaternion.identity) as GameObject;
		Destroy (newExplosion, Constants.ExplosionDuration);
		Destroy (item);
	}

	private GameObject GetRandomBlock ()
	{
		return BlockPrefabs [Random.Range (0, BlockPrefabs.Length)];
	}
		
	private void IncreaseScore (int amount)
	{
		score += amount;
		ShowScore ();
	}

	private void ShowScore ()
	{
		ScoreText.text = score.ToString ();
	}

	/// Get a random explosion
	private GameObject GetRandomExplosion ()
	{
		return ExplosionPrefabs [Random.Range (0, ExplosionPrefabs.Length)];
	}

	/// Gets the specified Bonus for the specific type
	private GameObject GetBonusFromType (string type)
	{
		string color = type.Split ('_') [1].Trim ();
		foreach (var item in BonusPrefabs) {
			if (item.GetComponent<Shape> ().Type.Contains (color))
				return item;
		}
		throw new System.Exception ("Wrong type");
	}

	public void rotateBoardCW ()
	{	
		parent.transform.Rotate (0, 0, 90);

		GameObject[,] ret = new GameObject[Constants.Rows, Constants.Columns];
		
		for (int i = 0; i < Constants.Rows; ++i) {
			for (int j = 0; j < Constants.Columns; ++j) {
				ret[i, j] = shapes[i, 8 - j - 1];
			}
		}
		shapes.shapes = ret;
		RotateCollapse ();
	}

	public void rotateBoardCCW ()
	{
		parent.transform.Rotate (0, 0, -90);
	}

	public IEnumerator RotateCollapse()
	{
		var collapsedBlockInfo = shapes.Collapse (Enumerable.Range(0,(Constants.Columns-1)));
		
		int maxDistance = Mathf.Max (collapsedBlockInfo.MaxDistance);
		
		MoveAndAnimate (collapsedBlockInfo.AlteredBlock, maxDistance);
		//will wait for both of the above animations
		yield return new WaitForSeconds (Constants.MoveAnimationMinDuration);
		
		//search if there are matches with the new/collapsed items
		var totalMatches = shapes.GetMatches (collapsedBlockInfo.AlteredBlock);
	}
}