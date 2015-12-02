using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public bool inputBlocked;
	public KeyCode up, down, left, right;										//Keys for Movement directions
	public KeyCode move, clear, shiftcw, shiftccw, attack1, attack2,swap;		//Keys for special inputs
	public ShapesManager sm;													//Pointer to player's Shapemanager
	public int x, y;															//Used to make sure row and column are within board
	public GameObject cursor;													//Pointer to cursor on player's board

	private bool pause;															//Boolean for Assassin power1
	private KeyCode temp;														//Temporary Key
	private float tempnum;														//Temporary Number
		
	void Start ()
	{
		inputBlocked = false;
		x = 3;
		y = 3;
	}

	void Update ()
	{

		if (!inputBlocked) {
			if (!pause) {
				//Move Cursor Left
				if (Input.GetKeyDown (left)){
					if (cursor.transform.localPosition.x > -3) {  
						cursor.transform.localPosition += Vector3.left;
						x -= 1;
					}
		
					//Move Cursor Right
				} else if (Input.GetKeyDown (right)) {
					if (cursor.transform.localPosition.x < 3) { 
						cursor.transform.localPosition += Vector3.right;
						x += 1;
					}

					//Move Cursor Up
				} else if (Input.GetKeyDown (up)) {
					if (cursor.transform.localPosition.y < 3) { 
						cursor.transform.localPosition += Vector3.up;
						y += 1;
					}
		
					//Move Cursor Down
				} else if (Input.GetKeyDown (down)) {
					if (cursor.transform.localPosition.y > -3) {
						cursor.transform.localPosition += Vector3.down;
						y -= 1;
					}
				} 

		//Activate Special Attack #1
		else if (Input.GetKeyDown (attack1)) {
					sm.checkAttack (1);
				}

		//Activate Special Attack #2
		else if (Input.GetKeyDown (attack2)) {
					sm.checkAttack (2);
				}

		//Activate Clearance of board and regenerate
		else if (Input.GetKeyDown (clear)) {
					sm.ResetBoard ();
				}

		//Activate Clockwise Rotation of board (WIP)
		else if (Input.GetKeyDown (shiftcw)) {
					sm.rotateBoardCW ();
			
				}

		//Activate CounterClockwise Rotation of board (WIP)
		else if (Input.GetKeyDown (shiftccw)) {
					sm.rotateBoardCCW ();
				}
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
			pause = false;
		} else {
			pause = true;
		}
	}
}