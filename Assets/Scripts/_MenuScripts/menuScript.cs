using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class menuScript : MonoBehaviour {

	public GameObject bg;
	public GameObject[] screens;
	public Sprite[] screenBG;
	public cursorInput p1,p2;
	public bool ready;

	private int lastScreen = 0;
	
	// Use this for initialization
	void Start () {
		loadScreen (0, 0);
	}

	void Update() {
		if (p1.character > -1 && p2.character > -1) {
			ready = true;
		} else {
			ready = false;
		}
	}

	public void loadScreen(int oldS, int newS){
		lastScreen = oldS;

		if (newS == -2) {
			newS = lastScreen;
		}
		screens [oldS].transform.position = new Vector3 (100, 100, 0);
		screens [oldS].SetActive (false);
		if (newS <= 3) {
			var b = bg.GetComponent<SpriteRenderer> ();
			b.sprite = screenBG [newS];
		}
		screens [newS].transform.position = Vector3.zero;
		screens [newS].SetActive (true);
	}

	public void ExitGame(){
		Application.Quit (); 
	}



}
