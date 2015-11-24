using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class startFight : MonoBehaviour {
	public Button b; 
	public Image selection; 
	public Sprite img; 

	public GameObject p1;
	public GameObject p2; 

	void Start(){
		b.enabled = false; 
	}

	public void OnMouseDown(){
		Application.LoadLevel ("Game");
	}

	void Update(){
		playerReady player1 =  p1.GetComponent<playerReady>();
		playerReady player2 =  p2.GetComponent<playerReady>();

		if (player1.p_ready && player2.p_ready) {
			selection.sprite = img;

			PlayerPrefs.SetString("char1",player1.character);
			PlayerPrefs.SetString("char2",player2.character);
			b.enabled = true; 
		}
	}
	
}
