using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public bool inputBlocked;
	public GameManager gm;
	public KeyCode up, down, left, right;										//Keys for Movement directions
	public KeyCode move, clear, shiftcw, shiftccw, swap, attack1, attack2, pause;//Keys for special inputs
	public ShapesManager sm;													//Pointer to player's Shapemanager
	public int x, y;															//Used to make sure row and column are within board
	public GameObject cursor;													//Pointer to cursor on player's board

	public bool paused;														//Boolean for Assassin power1
	private KeyCode temp;														//Temporary Key
	private float tempnum;														//Temporary Number
	private defaultControls dc = new defaultControls();

	void Start ()
	{
		inputBlocked = false;
		x = 3;
		y = 3;
		dc.setControls (sm.player);
		swap = dc.swap;
	}

	void Update ()
	{
		if (Input.GetKeyDown (pause)) {
			if (Time.timeScale == 1) {
				gm.paused = true;
				inputBlocked = true;
			} else {
				gm.paused = false;
				inputBlocked = false;
			}
		}

		if (gm.paused) {
			inputBlocked = true;
		} else {
			inputBlocked = false;
		}

		if (!inputBlocked) {

//			if (cursor.transform.localPosition.x < 3) {
//				if (Input.GetAxis ("L_XAxis_" + sm.player.ToString ()) == 1.0f) {
//					cursor.transform.localPosition += Vector3.right;				
//				}
//			}
//			if (cursor.transform.localPosition.x > -3) {
//				if (Input.GetAxis ("L_XAxis_" + sm.player.ToString ()) == 1.0f) {
//					cursor.transform.localPosition += Vector3.left;
//				}
//			}
//			if (cursor.transform.localPosition.y > -3.5) {
//				if (Input.GetAxis ("L_YAxis_" + sm.player.ToString ()) == 1.0f) {
//					cursor.transform.localPosition += Vector3.down;
//				}
//			}
//			if (cursor.transform.localPosition.y < 3.5) {
//				if (Input.GetAxis ("L_YAxis_" + sm.player.ToString ()) < -0.75f) {
//					cursor.transform.localPosition += Vector3.up;				}
//			}
		
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
				if (Input.GetAxis ("DPad_XAxis_" + sm.player.ToString ()) == 1.0f) {
					cursor.transform.localPosition += Vector3.right;
					while (Input.GetAxis ("DPad_XAxis_"+sm.player.ToString()) > 0) {
					}
				}
				if (Input.GetAxis ("DPad_XAxis_" + sm.player.ToString ()) == -1.0f) {
					cursor.transform.localPosition += Vector3.left;
					while (Input.GetAxis ("DPad_XAxis_"+sm.player.ToString()) < 0) {
					}
				}
				if (Input.GetAxis ("DPad_YAxis_" + sm.player.ToString ()) == 1.0f) {
					cursor.transform.localPosition += Vector3.up;
					while (Input.GetAxis ("DPad_YAxis_"+sm.player.ToString()) > 0) {
					}
				}
				if (Input.GetAxis ("DPad_YAxis_" + sm.player.ToString ()) == -1.0f) {
					cursor.transform.localPosition += Vector3.down;
					while (Input.GetAxis ("DPad_YAxis_"+sm.player.ToString()) < 0) {
					}
				}
			}

			//Move Cursor Left
			if (Input.GetKeyDown (dc.left)) {
				if (cursor.transform.localPosition.x > -3) {  
					cursor.transform.localPosition += Vector3.left;
					x -= 1;
				}
		
				//Move Cursor Right
			} else if (Input.GetKeyDown (dc.right)) {
				if (cursor.transform.localPosition.x < 3) { 
					cursor.transform.localPosition += Vector3.right;
					x += 1;
				}

				//Move Cursor Up
			} else if (Input.GetKeyDown (dc.up)) {
				if (cursor.transform.localPosition.y < 3) { 
					cursor.transform.localPosition += Vector3.up;
					y += 1;
				}
		
				//Move Cursor Down
			} else if (Input.GetKeyDown (dc.down)) {
				if (cursor.transform.localPosition.y > -3) {
					cursor.transform.localPosition += Vector3.down;
					y -= 1;
				}
			}

			//Activate Special Attack #1
			else if (Input.GetKeyDown (dc.attack1)) {
				sm.checkAttack (1);
			}

			//Activate Special Attack #2
			else if (Input.GetKeyDown (dc.attack2)) {
				sm.checkAttack (2);
			}

			//Activate Clearance of board and regenerate
			else if (Input.GetKeyDown (dc.clear)) {
				sm.ResetBoard ();
			}

			//Activate Clockwise Rotation of board (WIP)
			else if (Input.GetKeyDown (dc.shiftcw)) {
				sm.rotateBoardCW ();
			}
			//Activate CounterClockwise Rotation of board (WIP)
			else if (Input.GetKeyDown (dc.shiftccw)) {
				sm.rotateBoardCCW ();
			}
		}
	}

	//Return screen position of player's cursor
	public Vector2 getCursorLocation ()
	{
		return cursor.transform.position;
	} 

	public void pauseCursor(bool p){
		if (p) {
			paused = false;
		} else {
			paused = true;
		}
	}
}