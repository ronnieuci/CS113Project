using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class winScreen : MonoBehaviour {

	public GameObject emb,msg,pic,bg;
	public Sprite[] m,t,p,b;
	private int player,character;

	// Use this for initialization
	void Start () {
		player = PlayerPrefs.GetInt ("winner");
		print (player);
		character = PlayerPrefs.GetInt ("CharWin");
		print(character);

		var a = msg.GetComponent<SpriteRenderer> ();
		a.sprite = m [player];
		var e = emb.GetComponent<SpriteRenderer> ();
		e.sprite = t [character];
		var c = pic.GetComponent<SpriteRenderer> ();
		c.sprite = p [character];
		var d = bg.GetComponent<SpriteRenderer> ();
		d.sprite  = b [character];
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
