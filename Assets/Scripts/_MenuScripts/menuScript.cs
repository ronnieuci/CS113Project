using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Canvas characterSelectionMenu; 
	public Canvas instructionBox;
	public Button exitText; 


	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> (); 
		startText = startText.GetComponent<Button> ();
		characterSelectionMenu = characterSelectionMenu.GetComponent<Canvas> (); 
		instructionBox = instructionBox.GetComponent<Canvas> (); 
		exitText = exitText.GetComponent<Button> (); 
		quitMenu.enabled = false; 
		instructionBox.enabled = false; 
		characterSelectionMenu.enabled = false;
	}




	public void VersusPress(){
		characterSelectionMenu.enabled = true; 
		quitMenu.enabled = false; 
		instructionBox.enabled = false;
		startText.enabled = false; 
		exitText.enabled = false; 
	}

	public void InstructionPress(){
		quitMenu.enabled = false; 
		instructionBox.enabled = true;
		startText.enabled = false; 
		exitText.enabled = false; 
		characterSelectionMenu.enabled = false;
	}



	public void ExitPress(){
		quitMenu.enabled = true; 
		instructionBox.enabled = false;
		startText.enabled = false; 
		exitText.enabled = false; 
		characterSelectionMenu.enabled = false;
	}

	public void NoPress(){
		quitMenu.enabled = false; 
		instructionBox.enabled = false;
		startText.enabled = true; 
		exitText.enabled = true; 	
		characterSelectionMenu.enabled = false;
	}

	public void StartGame(){
	//
	}

	public void ExitGame(){
		Application.Quit (); 
	}


}
