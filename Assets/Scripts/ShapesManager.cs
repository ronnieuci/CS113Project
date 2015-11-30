using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShapesManager : MonoBehaviour
{
	public AudioClip blockDrop,blockSlide;															//Sounds for various events
	public Characters[] playerChar = new Characters[3];												//Array for characters player is using
	public GameObject NullBlock, gm;																//Instance of NullBlock(Used for shape movement)
	public GameObject[] BlockPrefabs, ExplosionPrefabs, BonusPrefabs;								//List of Blocks, Special Blocks, and explosion instances
	public GameObject BG,cursor;																	//Reference to background and cursor for each character
	public int score,player;																		//Integer to store score
	public PlayerInput play;																		//Instance for player's inputs
	public ShapesArray shapes;																		//Instance of shape array
	public Shader greyscale;
	public SoundManager sound;																		//Instance of sound player
	public Text ScoreText;																			//On-Screen Text for score (Not currently on screen)
	public Transform parent;																		//Parent pointer for Blocks
	public Vector2 middlePoint;																		//Middle-point of the gameboard
	public Vector2 BlockSize = new Vector2 (1.0f, 1.0f);											//Size of each block

	private IEnumerable<GameObject> totalMatches;
	private bool assnPower,magePower;																//Used for block and board placement
	private GameState state = GameState.None;														//Default GameState (used to split up coding in Update)
	private GameObject hitGo = null;																//Reference for block one when swapping blocks with cursor
	private GameObject hitGo2 = null;																//Reference for block two when swapping blocks with cursor		
	private int g1,g2;																				//Integer to track number of Special attack gems accumulated
	private int[] ch = new int[3];																	//Integer Array to store character choices in menu
	private SpriteRenderer backg;																	//Reference to Background for each character
	private Vector2 swapDirection1, swapDirection2;													//Directions that cursor checks
	private Vector2[] SpawnPositions;																//No Longer in use (needs to be removed, but currently breaks)


	void Awake ()
	{
		swapDirection1 = Vector2.right;
		swapDirection2 = Vector2.left;
		backg = BG.GetComponent<SpriteRenderer>();
		g1 = 0;
		g2 = 0;
	}

	void Start ()
	{
		ch [1] = gm.GetComponent<GameManager> ().loadCharacter (player);
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
			playerChar[m].setBlockBonus(SetCharBlocks(i));
			m+=1;
		}

		if (parent.position.x > 0) 
		{ }
		backg.sprite = playerChar [1].BG;
		cursor.GetComponent<SpriteRenderer> ().color = playerChar [1].charColor;
		cursor.GetComponent<SpriteRenderer> ().color = new Color (playerChar [1].charColor.r, playerChar [1].charColor.g, playerChar [1].charColor.b, playerChar [1].charColor.a + 5.5f);
	}

	// Update is called once per frame
	void FixedUpdate (){
		
		playerChar [1].animatetheGems(g1,g2);

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
					hitGo2 = hit.collider.gameObject;
					StartCoroutine (FindMatches (true));
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
					hitGo2 = hit.collider.gameObject;
					StartCoroutine (FindMatches (true));
				}
			}
		}
	}

	private IEnumerator FindMatches (bool match) {
		Shape hitGoCache = null;
		bool addBonus = false;

		if (match) {
			//move the swapped ones
			sound.PlaySingle (blockSlide);
			shapes.Swap (hitGo, hitGo2);
			hitGo.transform.positionTo (Constants.AnimationDuration, hitGo2.transform.position);
			hitGo2.transform.positionTo (Constants.AnimationDuration, hitGo.transform.position);
			yield return new WaitForSeconds (Constants.AnimationDuration);
		
			//get the matches via the helper methods
			var hitGomatchesInfo = shapes.GetMatches (hitGo);
			var hitGo2matchesInfo = shapes.GetMatches (hitGo2);
			totalMatches = hitGomatchesInfo.MatchedBlock.Union (hitGo2matchesInfo.MatchedBlock).Distinct ();
		
			//if more than 3 matches and no Bonus is contained in the line, we will award a new Bonus
			addBonus = totalMatches.Count () >= Constants.MinimumMatchesForBonus &&
				!BonusTypeUtilities.ContainsDestroyWholeRowColumn (hitGomatchesInfo.BonusesContained) &&
				!BonusTypeUtilities.ContainsDestroyWholeRowColumn (hitGo2matchesInfo.BonusesContained);
		

			if (addBonus) {
				hitGoCache = new Shape ();
				//get the game object that was of the same type
				var sameTypeGo = hitGomatchesInfo.MatchedBlock.Count () > 0 ? hitGo : hitGo2;
				var shape = sameTypeGo.GetComponent<Shape> ();

				//cache it
				hitGoCache.Assign (shape.Type, shape.Row, shape.Column);
			}
		
			if (hitGo.GetComponent<Shape> ().IsSameType (NullBlock.GetComponent<Shape> ())) {
				shapes.setNullBlock (hitGo);
				Destroy (hitGo);
			} 
		
			if (hitGo2.GetComponent<Shape> ().IsSameType (NullBlock.GetComponent<Shape> ())) {
				shapes.setNullBlock (hitGo2);
				Destroy (hitGo2);
			}
		} else {
			var blankMatches = shapes.GetMatches(NullBlock);
			totalMatches = blankMatches.MatchedBlock;
		}
		int timesRun = 0;
	
	RESTART:	
		if (totalMatches.Count () >= Constants.MinimumMatches) {
			//increase score
			
			if (!assnPower){
				IncreaseScore ((totalMatches.Count () - 2) * Constants.Match3Score);
			}
			else {
				IncreaseScore ((totalMatches.Count () - 2) * (Constants.Match3Score-2));
			}

			if (timesRun >= 1){
				IncreaseScore (Constants.SubsequentMatchScore);
				assnPower=false;
			}
			
			foreach (var item in totalMatches) {
				if (item != null) {
					if (!assnPower)
					{
						if (item.GetComponent<Shape> ().IsSameType (playerChar [1].bonus [0].GetComponent<Shape> ())) {
							g1 += 1;
						}
						else if (item.GetComponent<Shape> ().IsSameType (playerChar [1].bonus [1].GetComponent<Shape> ())) {
							g2 += 1;
						}
					}
					shapes.Remove (item);
					RemoveFromScene (item);
				}
			}

			//check and instantiate Bonus if needed
			if (addBonus) {
				CreateBonus (hitGoCache);
				addBonus = false;
			}

			//collapse the ones gone
			var collapsedBlockInfo = shapes.Collapse (Enumerable.Range (0, 8));
			MoveAndAnimate (collapsedBlockInfo.AlteredBlock, collapsedBlockInfo.MaxDistance);
			yield return new WaitForSeconds (Constants.MoveAnimationMinDuration*collapsedBlockInfo.MaxDistance );

			//search if there are matches with the new/collapsed items
			totalMatches = shapes.GetMatches (collapsedBlockInfo.AlteredBlock); //////CHECK FOR PIECES THAT ARE NOT MATCHED BUT STILL CHANGE

			timesRun++;
			goto RESTART;
		} else {
			var collapsedBlockInfo = shapes.Collapse (Enumerable.Range (0, 8));
			MoveAndAnimate (collapsedBlockInfo.AlteredBlock, collapsedBlockInfo.MaxDistance);
			yield return new WaitForSeconds (Constants.MoveAnimationMinDuration*collapsedBlockInfo.MaxDistance);
			sound.PlaySingle (blockDrop);

			//search if there are matches with the new/collapsed items
			totalMatches = shapes.GetMatches (collapsedBlockInfo.AlteredBlock);
			if (totalMatches.Count () >= Constants.MinimumMatches) {
				timesRun++;
				goto RESTART;
			}
		}
		totalMatches = null;
		state = GameState.None;
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

		
		var currPrefab = BlockPrefabs;

		if (magePower) {
			currPrefab = new GameObject[4];
			for (int a = 0; a < 4; a++) {
				var p = Random.Range (0, 6);
				currPrefab [a] = BlockPrefabs [p];
			}
		}
	
	
		SpawnPositions = new Vector2[Constants.Columns];
			
		for (int row = 0; row < Constants.Rows; row++) 
		{
			for (int column = 0; column < Constants.Columns; column++) 
			{
				GameObject newBlock = GetRandomBlock (currPrefab);
				
				//check if two previous horizontal are of the same type
				while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>()) 
				       			   && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())) 
				{
					newBlock = GetRandomBlock (currPrefab);
				}
				
				//check if two previous vertical are of the same type
				while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())
				       			&& shapes[row - 2, column].GetComponent<Shape>().IsSameType(newBlock.GetComponent<Shape>())) 
				{
					newBlock = GetRandomBlock (currPrefab);
				}
				InstantiateAndPlaceNewBlock (row, column, newBlock);
			}
		}
		SetupSpawnPositions ();
	}

	private void InstantiateAndPlaceNewBlock (int row, int column, GameObject newBlock)
	{
		GameObject go = Instantiate (newBlock, new Vector2 (parent.transform.position.x+ middlePoint.x + (column * BlockSize.x),parent.transform.position.y+ middlePoint.y + (BlockSize.y * row)), Quaternion.identity) as GameObject;

		//assign the specific properties
		go.GetComponent<Shape> ().Assign (newBlock.GetComponent<Shape> ().Type, row, column);
		shapes [row, column] = go;
		go.transform.parent = parent;
	}
	
	private void SetupSpawnPositions ()
	{
	}

	/// Destroy all Block gameobjects and resets board
	public void ResetBoard ()
	{
		for (int row = 0; row < Constants.Rows; row++) {
			for (int column = 0; column < Constants.Columns; column++) {
				if (shapes [row, column] != null)
					RemoveFromScene (shapes [row, column]);
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

	/// Creates a new Bonus based on the shape parameter
	private void CreateBonus (Shape hitGoCache)
	{
		GameObject Bonus = Instantiate (GetBonusFromType (hitGoCache.Type), middlePoint
			+ new Vector2 ((hitGoCache.Column * BlockSize.x) + parent.transform.position.x,
		              	  + (hitGoCache.Row * BlockSize.y) + parent.transform.position.y), Quaternion.identity)
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
			if (item != null) {
				item.transform.positionTo (Constants.MoveAnimationMinDuration * distance, middlePoint + new Vector2 (((item.GetComponent<Shape> ().Column * BlockSize.x) + parent.transform.position.x), ( (item.GetComponent<Shape> ().Row * BlockSize.y)) + parent.transform.position.y));
			}
		}
	}

	private void RemoveFromScene (GameObject item)
	{
		GameObject explosion = GetRandomExplosion ();
		var newExplosion = Instantiate (explosion, item.transform.position, Quaternion.identity) as GameObject;
		Destroy (newExplosion, Constants.ExplosionDuration);
		Destroy (item);
	}

	private GameObject GetRandomBlock (GameObject[] l)
	{
		return l [Random.Range (0, l.Length)];
	}

	private GameObject[] SetCharBlocks (int character)
	{
		GameObject[] a = new GameObject[2];

		if (character == 1) {
			a [0] = GetBlock (5);
			a [1] = GetBlock (1);
		} else if (character == 2) {
			a [0] = GetBlock (0);
			a [1] = GetBlock (3);
		} else if (character == 3) {
			a [0] = GetBlock (4);
			a [1] = GetBlock (2);
		} else {
			a [0] = GetBlock (0);
			a [1] = GetBlock (0);
		}
		return a;
	}

	private GameObject GetBlock (int i)
	{
		var a = BlockPrefabs[i];
		a.GetComponent<Shape> ().Type = a.name;
		return a;
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
				
	public void checkAttack(int i){	
		if (i == 1 && playerChar[1].power1) {
			g1=0;
			if(ch[1]==1) {
				playerChar[1].assassinatk1();
			}
			else if(ch[1]==2) {
				var effect = playerChar[1].effect1;
				var a = gm.GetComponent<GameManager>().getRandomBlocks(player);
				attack1 (a,effect);
			}
			else if(ch[1]==3) {
				var a = gm.GetComponent<GameManager>().getAllBlocks(player);
				foreach(var block in a){
					var b = block.GetComponent<SpriteRenderer>();
					b.material.shader =  greyscale;
				}
			}
		}
		else if (i == 2 && playerChar[1].power2) {
			g2=0;
			if(ch[1]==1) {
				var opp = gm.GetComponent<GameManager>().getOtherPlayer(player);
				opp.assnPower= true;
				if(countDown(15.0f)){
					opp.assnPower= false;
				}
			}
			else if(ch[1]==2) {
				magePower = true;
				ResetBoard();
				magePower = false;
			}
			if(ch[1]==3) {
				totalMatches = shapes.getAllBlocksOfColor(GetRandomBlock(BlockPrefabs));
				StartCoroutine(FindMatches(false));
			}
		}
	}

	public void attack1(IEnumerable<GameObject> a, int effect){
		foreach (GameObject block in a) {
			var b = block.GetComponent<SpriteRenderer>();
			b.color = playerChar[1].fxColor;
			block.GetComponent<Shape>().setStatus(effect);
		}
		playerChar[1].sprite.GetComponent<Animator> ().Play ("Attack");}

	public bool countDown(float seconds){
		float time = seconds;
		while (time>0) {
			time -= Time.deltaTime;
		}
		return true;
	}

	public void rotateBoardCW ()
	{	
		shapes.RotateCW ();
		parent.transform.Rotate (0, 0, -90);
		StartCoroutine (FindMatches(false));

	}
	
	public void rotateBoardCCW ()
	{
		shapes.RotateCCW ();
		parent.transform.Rotate (0, 0, 90);
		StartCoroutine (FindMatches (false));
		holdingTime (2.0f);
	}

	public IEnumerator holdingTime(float time)
	{
		play.inputBlocked = true;
		yield return new WaitForSeconds (time);
		play.inputBlocked = false;
	}
}