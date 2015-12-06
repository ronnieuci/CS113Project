using UnityEngine;
using System.Collections;

public class winScreen : MonoBehaviour {

	public GameObject emb,msg,pic,bg;
	public Sprite[] m,t,p,b;
	private int player,character;

	// Use this for initialization
	void Start () {
		player = PlayerPrefs.GetInt ("winner");
		character = PlayerPrefs.GetInt ("CharWin");

		msg.GetComponent<SpriteRenderer> ().sprite = m [player];
		emb.GetComponent<SpriteRenderer> ().sprite = t [character];
		pic.GetComponent<SpriteRenderer> ().sprite = p [character];
		bg.GetComponent<SpriteRenderer> ().sprite  = b [character];
	}

	public void action(bool c){
		if (c) {
			PlayerPrefs.SetInt("Select",1);
			Application.LoadLevel("MainMenu");
		}else{
			Application.Quit();
		}
	}
}
