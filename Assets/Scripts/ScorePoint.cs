using UnityEngine;
using System.Collections;

public class ScorePoint : MonoBehaviour {

	public int winScore;					//Score Needed to Win the Game
	public ShapesManager p1,p2;				//Used to Reference Player's game instance
	public ScorePoint score;				//Tab/Arrow on Scoreboard
	private bool gameOver;					//Is the Game over?

	//Set Default settings
	void Start () {
		score.transform.localPosition = new Vector3 (0, -.05f, 0);
		gameOver = false;
	}

	void Update () {

		//If game is still going...
		if (!gameOver) 
		{
			//Find difference in score
			float x = ((p2.score - p1.score));

			//Move Arrow based on the score
			score.transform.localPosition = new Vector3 (((6 * x) / winScore), -.05f, 0);
		
			//Determine if game is over, and if it is, who wins?
			if ((p2.score - p1.score) > winScore) {
				score.transform.localPosition = new Vector3 (6, -.05f, 0);
				print ("Player 2 Wins!");
				gameOver=true;
			} else if ((p2.score - p1.score) < -winScore) {
				score.transform.localPosition = new Vector3 (-6, -.05f, 0);
				print ("Player 1 Wins!");
				gameOver=true;
			}
		}
	}
}
