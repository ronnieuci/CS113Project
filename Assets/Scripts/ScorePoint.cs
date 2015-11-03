using UnityEngine;
using System.Collections;

public class ScorePoint : MonoBehaviour {

	public int winScore;
	public ShapesManager p1,p2;
	public ScorePoint score;
	private bool gameOver;
	// Use this for initialization

	void Start () {
		score.transform.localPosition = new Vector3 (0, -.05f, 0);
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameOver) 
		{
			float x = ((p2.score - p1.score));
			score.transform.localPosition = new Vector3 (((6 * x) / winScore), -.05f, 0);
		
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
