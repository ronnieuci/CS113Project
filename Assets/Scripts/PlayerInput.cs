using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public KeyCode up, down, left, right;										//Keys for Movement directions
	public KeyCode move, clear, shiftcw, shiftccw, swap, attack1, attack2;		//Keys for special inputs
	public ShapesManager sm;													//Pointer to player's Shapemanager
	public int x, y;															//Used to make sure row and column are within board
	public GameObject cursor;													//Pointer to cursor on player's board

	private KeyCode temp;														//Temporary Key
	private float tempnum;														//Temporary Number
		
	void Start ()
	{
		x = 3;
		y = 3;
	}

	void Update ()
	{
		//Move Cursor Left
		if (Input.GetKeyDown (left)) {
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
			sm.attack1(1);
		}

		//Activate Special Attack #2
		else if (Input.GetKeyDown (attack2)) {
			sm.attack2(1);
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

	//Return screen position of player's cursor
	public Vector2 getCursorLocation ()
	{
		return cursor.transform.position;
	} 
}