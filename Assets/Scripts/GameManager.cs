using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public ShapesManager player1,player2;
	public float pscore1, pscore2;
	public int scoreToWin, volume;
	public int p1,p2;
	public bool powerEnabled,paused;
	public SoundManager sound;

	void Start(){
		getSettings ();
		print (Application.platform);
	}

	public void getSettings(){
		p1 = PlayerPrefs.GetInt ("char1");
		p2 = PlayerPrefs.GetInt ("char2");
		scoreToWin = PlayerPrefs.GetInt ("toWin");
		volume = PlayerPrefs.GetInt ("volume");
		usePower(); 
	}

	public void usePower() {
		var s = PlayerPrefs.GetString ("powers");
		if (s.ToLower () == "true") {
			powerEnabled = true;
		} else { 
			powerEnabled = false;
		}
	}

	public int getChar(int player){
		if (player == 1) {
			return p1;
		} else if (player == 2) {
			return p2;
		} else {
			return 0;
		}
	}

	public IEnumerable<GameObject> getRandomBlocks(int attacker) {
		if (attacker == 1) {
			return( player2.shapes.getRandomBlocks(8));
		} 
		else {
			return( player1.shapes.getRandomBlocks(8));
		}
	}

	public IEnumerable<GameObject> getAllBlocks(int attacker) {
		if (attacker == 1) {
			return( player2.shapes.GetAllBlocks());
		} 
		else {
			return( player1.shapes.GetAllBlocks());
		}
	}

	public ShapesManager getOtherPlayer(int attacker) {
		if (attacker == 1) {
			return(player2);
		} 
		else {
			return(player1);
		}
	}

	void Update(){
		if (paused) {
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1;
		}
	}
}