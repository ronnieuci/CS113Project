using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class menuScript : MonoBehaviour {

	public GameObject bg;
	public GameObject[] screens;
	public Sprite[] screenBG;
	public cursorInput p1,p2;
	public bool ready,powers;
	public int winScore, volNum;
	public settingMenuCounts sm;
	
	// Use this for initialization
	void Start () {
		loadScreen (0, 0);
		ready = false;
		powers = true;
		winScore = 100;
		volNum = 50;

		if (PlayerPrefs.GetInt ("Select") == 1) {
			this.loadScreen (0, 1);
			PlayerPrefs.SetInt ("Select", 0);
		}
	}

	void Update() {
		if (p1.character > -1 && p2.character > -1) {
			ready = true;
		} else {
			ready = false;
		}
	}

	public void loadScreen(int oldS, int newS){
		if (newS == 10) {
			ExitGame ();
		}
		if (oldS == 4) {
			winScore = sm.getPoints();
			volNum = sm.volNum;
		}
		screens [oldS].transform.position = new Vector3 (100, 100, 0);
		screens [oldS].SetActive (false);
		if (newS <= 5) {
			var b = bg.GetComponent<SpriteRenderer> ();
			b.sprite = screenBG [newS];
		}
		screens [newS].transform.position = Vector3.zero;
		screens [newS].SetActive (true);
	}

	public void StartGame(){
		passSettings ();
		Application.LoadLevel ("Game");
	}

	public void ReloadGame(){
		Application.LoadLevel ("MainMenu");
	}

	public void ExitGame(){
		Application.Quit (); 
	}

	private void passSettings()
	{
		var player1 = p1.GetComponent<cursorInput> ();
		var player2 = p2.GetComponent<cursorInput> ();
		
		PlayerPrefs.SetInt ("char1", player1.character);
		PlayerPrefs.SetInt("char2", player2.character);
		PlayerPrefs.SetInt ("toWin", winScore);
		PlayerPrefs.SetInt ("volume", volNum);
		PlayerPrefs.SetString ("powers", powers.ToString ()); 
	}
}
