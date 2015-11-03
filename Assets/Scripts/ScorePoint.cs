using UnityEngine;
using System.Collections;

public class ScorePoint : MonoBehaviour {

	public int winScore;
	public ShapesManager p1,p2;
	public ScorePoint score;
	// Use this for initialization
	void Start () {
		score.transform.localPosition = new Vector3 (0, -.05f, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
		float x = ((p2.score - p1.score));
		score.transform.localPosition = new Vector3(x,-.05f, 0);

		if ((p2.score - p1.score) > winScore) {
			print ("Player 2 Wins!");
		}
		else if ((p2.score - p1.score) < -winScore) {
			print ("Player 1 Wins!");
		}
	}
}
