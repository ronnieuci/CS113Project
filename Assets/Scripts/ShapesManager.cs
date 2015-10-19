using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShapesManager : MonoBehaviour
{
	public Text DebugText, ScoreText;
	public bool ShowDebugInfo = false;
	public ShapesArray shapes;
	public PlayerInput play;
	public Camera cam;
	
	public readonly Vector2 BottomRight = new Vector2(0, 0);
	public readonly Vector2 BlockSize = new Vector2(1, 1);

	private int score;
	private GameState state = GameState.None;
	private GameObject hitGo = null;
	private Vector2[] SpawnPositions;
	public GameObject[] BlockPrefabs;
	public GameObject[] ExplosionPrefabs;
	public GameObject[] BonusPrefabs;
	
	private IEnumerator CheckPotentialMatchesCoroutine;
	private IEnumerator AnimatePotentialMatchesCoroutine;
	
	IEnumerable<GameObject> potentialMatches;
	
	public SoundManager soundManager;
	void Awake()
	{
		DebugText.enabled = ShowDebugInfo;
	}
	
	// Use this for initialization
	void Start()
	{
		InitializeTypesOnPrefabShapesAndBonuses();
		
		InitializeBlockAndSpawnPositions();
		
		StartCheckForPotentialMatches();
	}

	/// Initialize shapes
	private void InitializeTypesOnPrefabShapesAndBonuses()
	{
		//just assign the name of the prefab
		foreach (var item in BlockPrefabs)
		{
			item.GetComponent<Shape>().Type = item.name;
			
		}
		
		//assign the name of the respective "normal" candy as the type of the Bonus
		foreach (var item in BonusPrefabs)
		{
			item.GetComponent<Shape>().Type = BlockPrefabs.
				Where(x => x.GetComponent<Shape>().Type.Contains(item.name.Split('_')[1].Trim())).Single().name;
		}
	}
	
	public void InitializeBlockAndSpawnPositionsFromPremadeLevel()
	{
		InitializeVariables();
		
		var premadeLevel = DebugUtilities.FillShapesArrayFromResourcesData();
		
		if (shapes != null)
			DestroyAllBlocks();
		
		shapes = new ShapesArray();
		SpawnPositions = new Vector2[Constants.Columns];
		
		for (int row = 0; row < Constants.Rows; row++)
		{
			for (int column = 0; column < Constants.Columns; column++)
			{
				
				GameObject newBlock = null;
				
				newBlock = GetSpecificBlockOrBonusForPremadeLevel(premadeLevel[row, column]);
				
				InstantiateAndPlaceNewBlock(row, column, newBlock);
			}
		}
		
		SetupSpawnPositions();
	}
	
	
	public void InitializeBlockAndSpawnPositions()
	{
		InitializeVariables();
		
		if (shapes != null)
			DestroyAllBlocks();
		
		shapes = new ShapesArray();
		SpawnPositions = new Vector2[Constants.Columns];
		
		for (int row = 0; row < Constants.Rows; row++)
		{
			for (int column = 0; column < Constants.Columns; column++)
			{
				
				GameObject newBlock = GetRandomBlock();
				
				//check if two previous horizontal are of the same type
				while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>()
				       .IsSameType(newBlock.GetComponent<Shape>())
				       && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>()))
				{
					newBlock = GetRandomBlock();
				}
				
				//check if two previous vertical are of the same type
				while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>()
				       .IsSameType(newBlock.GetComponent<Shape>())
				       && shapes[row - 2, column].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>()))
				{
					newBlock = GetRandomBlock();
				}
				
				InstantiateAndPlaceNewBlock(row, column, newBlock);
				
			}
		}
		
		SetupSpawnPositions();
	}
	
	
	
	private void InstantiateAndPlaceNewBlock(int row, int column, GameObject newBlock)
	{
		GameObject go = Instantiate(newBlock,
		                            BottomRight + new Vector2(column * BlockSize.x, row * BlockSize.y), Quaternion.identity)
			as GameObject;
		
		//assign the specific properties
		go.GetComponent<Shape>().Assign(newBlock.GetComponent<Shape>().Type, row, column);
		shapes[row, column] = go;
	}
	
	private void SetupSpawnPositions()
	{
		//create the spawn positions for the new shapes (will pop from the 'ceiling')
		for (int column = 0; column < Constants.Columns; column++)
		{
			SpawnPositions[column] = BottomRight
				+ new Vector2(column * BlockSize.x, Constants.Rows * BlockSize.y);
		}
	}


	/// Destroy all Block gameobjects and resets board\
	public void ResetBoard()
	{
		for (int row = 0; row < Constants.Rows; row++)
		{
			for (int column = 0; column < Constants.Columns; column++)
			{
				Destroy(shapes[row, column]);
			}
		}
		InitializeBlockAndSpawnPositions();
	}
	
	public void SetGravity(string direction)
	{
		if 		(direction.ToUpper() == "U"){ cam.transform.Rotate(new Vector3(0,0,90));Physics.gravity = new Vector3(0,-1, 0); }
		else if (direction.ToUpper() == "D"){ cam.transform.Rotate(new Vector3(0,0,90));Physics.gravity = new Vector3(0,1, 0);  }
		else if (direction.ToUpper() == "L"){ cam.transform.Rotate(new Vector3(0,0,90));Physics.gravity = new Vector3(-1,0, 0); }
		else if (direction.ToUpper() == "R"){ cam.transform.Rotate(new Vector3(0,0,90));Physics.gravity = new Vector3(1,0, 0);  }


	}

	/// <summary>
	/// Destroy all candy gameobjects
	/// </summary>
	private void DestroyAllBlocks()
	{
		for (int row = 0; row < Constants.Rows; row++)
		{
			for (int column = 0; column < Constants.Columns; column++)
			{
				Destroy(shapes[row, column]);
			}
		}
	}
	
	
	// Update is called once per frame
	void Update()
	{
		if (ShowDebugInfo)
			DebugText.text = DebugUtilities.GetArrayContents(shapes);

		if (state == GameState.None)
		{
			//user has clicked or touched
			if (Input.GetKey(KeyCode.Space))
			{
				//get the hit position
				var hit = Physics2D.Raycast(play.getCursorLocation(), Vector2.left);
				if (hit.collider != null) //we have a hit!!!
				{
					hitGo = hit.collider.gameObject;
					state = GameState.SelectionStarted;
				}
				
			}
		}
		else if (state == GameState.SelectionStarted)
		{
				var hit = Physics2D.Raycast(play.getCursorLocation(), Vector2.right);

				//we have a hit

				if (hit.collider == null)
				{			}
				else if(hit.collider != null && hitGo != hit.collider.gameObject)
				{
					//user did a hit, no need to show him hints 
					StopCheckForPotentialMatches();
					
					//if the two shapes are diagonally aligned (different row and column), just return
					if (!Utilities.AreVerticalOrHorizontalNeighbors(hitGo.GetComponent<Shape>(),
					                                                hit.collider.gameObject.GetComponent<Shape>()))
					{
						state = GameState.None;
					}
					else
					{
						state = GameState.Animating;
						FixSortingLayer(hitGo, hit.collider.gameObject);
						StartCoroutine(FindMatchesAndCollapse(hit));
					}
				}
			}
		}

	/// Modifies sorting layers for better appearance when dragging/animating
	private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
	{
		SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
		SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
		if (sp1.sortingOrder <= sp2.sortingOrder)
		{
			sp1.sortingOrder = 1;
			sp2.sortingOrder = 0;
		}
	}

	private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
	{
		//get the second item that was part of the swipe
		var hitGo2 = hit2.collider.gameObject;
		shapes.Swap(hitGo, hitGo2);
		
		//move the swapped ones
		hitGo.transform.positionTo(Constants.AnimationDuration, hitGo2.transform.position);
		hitGo2.transform.positionTo(Constants.AnimationDuration, hitGo.transform.position);
		yield return new WaitForSeconds(Constants.AnimationDuration);
		
		//get the matches via the helper methods
		var hitGomatchesInfo = shapes.GetMatches(hitGo);
		var hitGo2matchesInfo = shapes.GetMatches(hitGo2);
		
		var totalMatches = hitGomatchesInfo.MatchedBlock
			.Union(hitGo2matchesInfo.MatchedBlock).Distinct();

		//if more than 3 matches and no Bonus is contained in the line, we will award a new Bonus
		bool addBonus = totalMatches.Count() >= Constants.MinimumMatchesForBonus &&
			!BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGomatchesInfo.BonusesContained) &&
				!BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGo2matchesInfo.BonusesContained);
		
		Shape hitGoCache = null;
		if (addBonus)
		{
			hitGoCache = new Shape();
			//get the game object that was of the same type
			var sameTypeGo = hitGomatchesInfo.MatchedBlock.Count() > 0 ? hitGo : hitGo2;
			var shape = sameTypeGo.GetComponent<Shape>();
			//cache it
			hitGoCache.Assign(shape.Type, shape.Row, shape.Column);
		}
		int timesRun = 1;
		while (totalMatches.Count() >= Constants.MinimumMatches)
		{
			//increase score
			IncreaseScore((totalMatches.Count() - 2) * Constants.Match3Score);
			
			if (timesRun >= 2)
				IncreaseScore(Constants.SubsequentMatchScore);
			foreach (var item in totalMatches)
			{
				shapes.Remove(item);
				RemoveFromScene(item);
			}
			
			//check and instantiate Bonus if needed
			if (addBonus)
				CreateBonus(hitGoCache);
			
			addBonus = false;
			
			//get the columns that we had a collapse
			var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();
			
			//the order the 2 methods below get called is important!!!
			//collapse the ones gone
			var collapsedBlockInfo = shapes.Collapse(columns);
			//create new ones
			///var newBlockInfo = CreateNewBlockInSpecificColumns(columns);
			
			int maxDistance = Mathf.Max(collapsedBlockInfo.MaxDistance);
			MoveAndAnimate(collapsedBlockInfo.AlteredBlock, maxDistance);

			//will wait for both of the above animations
			yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);
			
			//search if there are matches with the new/collapsed items
			totalMatches = shapes.GetMatches(collapsedBlockInfo.AlteredBlock);

			timesRun++;
		}
		
		state = GameState.None;
		StartCheckForPotentialMatches();
	}

	/// Creates a new Bonus based on the shape parameter
	private void CreateBonus(Shape hitGoCache)
	{
		GameObject Bonus = Instantiate(GetBonusFromType(hitGoCache.Type), BottomRight
		                               + new Vector2(hitGoCache.Column * BlockSize.x,
		              hitGoCache.Row * BlockSize.y), Quaternion.identity)
			as GameObject;
		shapes[hitGoCache.Row, hitGoCache.Column] = Bonus;
		var BonusShape = Bonus.GetComponent<Shape>();
		//will have the same type as the "normal" candy
		BonusShape.Assign(hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
		//add the proper Bonus type
		BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;
	}

	/// Spawns new candy in columns that have missing ones
	private AlteredBlockInfo CreateNewBlockInSpecificColumns(IEnumerable<int> columnsWithMissingBlock)
	{
		AlteredBlockInfo newBlockInfo = new AlteredBlockInfo();
		
		//find how many null values the column has
		foreach (int column in columnsWithMissingBlock)
		{
			var emptyItems = shapes.GetEmptyItemsOnColumn(column);
			foreach (var item in emptyItems)
			{
				var go = GetRandomBlock();
				GameObject newBlock = Instantiate(go, SpawnPositions[column], Quaternion.identity)
					as GameObject;
				
				newBlock.GetComponent<Shape>().Assign(go.GetComponent<Shape>().Type, item.Row, item.Column);
				
				if (Constants.Rows - item.Row > newBlockInfo.MaxDistance)
					newBlockInfo.MaxDistance = Constants.Rows - item.Row;
				
				shapes[item.Row, item.Column] = newBlock;
				newBlockInfo.AddBlock(newBlock);
			}
		}
		return newBlockInfo;
	}
	
	/// <summary>
	/// Animates gameobjects to their new position
	/// </summary>
	/// <param name="movedGameObjects"></param>
	private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
	{
		foreach (var item in movedGameObjects)
		{
			item.transform.positionTo(Constants.MoveAnimationMinDuration * distance, BottomRight +
			                          new Vector2(item.GetComponent<Shape>().Column * BlockSize.x, item.GetComponent<Shape>().Row * BlockSize.y));
		}
	}
	
	/// <summary>
	/// Destroys the item from the scene and instantiates a new explosion gameobject
	/// </summary>
	/// <param name="item"></param>
	private void RemoveFromScene(GameObject item)
	{
		GameObject explosion = GetRandomExplosion();
		var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
		Destroy(newExplosion, Constants.ExplosionDuration);
		Destroy(item);
	}
	
	/// <summary>
	/// Get a random candy
	/// </summary>
	/// <returns></returns>
	private GameObject GetRandomBlock()
	{
		return BlockPrefabs[Random.Range(0, BlockPrefabs.Length)];
	}
	
	private void InitializeVariables()
	{
		score = 0;
		ShowScore();
	}
	
	private void IncreaseScore(int amount)
	{
		score += amount;
		ShowScore();
	}
	
	private void ShowScore()
	{
		ScoreText.text = "Score: " + score.ToString();
	}
	
	/// <summary>
	/// Get a random explosion
	/// </summary>
	/// <returns></returns>
	private GameObject GetRandomExplosion()
	{
		return ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)];
	}
	
	/// <summary>
	/// Gets the specified Bonus for the specific type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	private GameObject GetBonusFromType(string type)
	{
		string color = type.Split('_')[1].Trim();
		foreach (var item in BonusPrefabs)
		{
			if (item.GetComponent<Shape>().Type.Contains(color))
				return item;
		}
		throw new System.Exception("Wrong type");
	}
	
	/// <summary>
	/// Starts the coroutines, keeping a reference to stop later
	/// </summary>
	private void StartCheckForPotentialMatches()
	{
		StopCheckForPotentialMatches();
		//get a reference to stop it later
		CheckPotentialMatchesCoroutine = CheckPotentialMatches();
		StartCoroutine(CheckPotentialMatchesCoroutine);
	}
	
	/// <summary>
	/// Stops the coroutines
	/// </summary>
	private void StopCheckForPotentialMatches()
	{
		if (AnimatePotentialMatchesCoroutine != null)
			StopCoroutine(AnimatePotentialMatchesCoroutine);
		if (CheckPotentialMatchesCoroutine != null)
			StopCoroutine(CheckPotentialMatchesCoroutine);
		ResetOpacityOnPotentialMatches();
	}
	
	/// <summary>
	/// Resets the opacity on potential matches (probably user dragged something?)
	/// </summary>
	private void ResetOpacityOnPotentialMatches()
	{
		if (potentialMatches != null)
			foreach (var item in potentialMatches)
		{
			if (item == null) break;
			
			Color c = item.GetComponent<SpriteRenderer>().color;
			c.a = 1.0f;
			item.GetComponent<SpriteRenderer>().color = c;
		}
	}
	
	/// <summary>
	/// Finds potential matches
	/// </summary>
	/// <returns></returns>
	private IEnumerator CheckPotentialMatches()
	{
		yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
		potentialMatches = Utilities.GetPotentialMatches(shapes);
		if (potentialMatches != null)
		{
			while (true)
			{
				
				AnimatePotentialMatchesCoroutine = Utilities.AnimatePotentialMatches(potentialMatches);
				StartCoroutine(AnimatePotentialMatchesCoroutine);
				yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
			}
		}
	}
	
	/// <summary>
	/// Gets a specific candy or Bonus based on the premade level information.
	/// </summary>
	/// <param name="info"></param>
	/// <returns></returns>
	private GameObject GetSpecificBlockOrBonusForPremadeLevel(string info)
	{
		var tokens = info.Split('_');
		
		if (tokens.Count() == 1)
		{
			foreach (var item in BlockPrefabs)
			{
				if (item.GetComponent<Shape>().Type.Contains(tokens[0].Trim()))
					return item;
			}
			
		}
		else if (tokens.Count() == 2 && tokens[1].Trim() == "B")
		{
			foreach (var item in BonusPrefabs)
			{
				if (item.name.Contains(tokens[0].Trim()))
					return item;
			}
		}
		
		throw new System.Exception("Wrong type, check your premade level");
	}
}
