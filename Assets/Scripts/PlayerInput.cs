using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	
	//Keys
	public KeyCode up, down, left, right, move, clear, shiftcw, shiftccw, swap, temp;
	public ShapesManager sm;
	public float tempnum;
	public int x, y;
	public GameObject cursor;


	void Awake ()
	{}
	
	void Start ()
	{
		x = 3;
		y = 3;
	}

	void Update ()
	{
		if (Input.GetKeyDown (left)) {
			if (cursor.transform.localPosition.x > -3) {  
				cursor.transform.localPosition += Vector3.left;
				x -= 1;
			}
		} else if (Input.GetKeyDown (right)) {
			if (cursor.transform.localPosition.x < 3) { 
				cursor.transform.localPosition += Vector3.right;
				x += 1;
			}
		} else if (Input.GetKeyDown (up)) {
			if (cursor.transform.localPosition.y < 3) { 
				cursor.transform.localPosition += Vector3.up;
				y += 1;
			}
		}
		else if (Input.GetKeyDown (down)) {
			if (cursor.transform.localPosition.y> -3) {
				cursor.transform.localPosition += Vector3.down;
				y -= 1;
			}
		}
		else if (Input.GetKeyDown (clear)) {
			sm.ResetBoard ();
			
		}
		else if (Input.GetKeyDown (shiftcw)) {
			sm.rotateBoardCW ();
			
		}
		else if (Input.GetKeyDown (shiftccw)) {
			sm.rotateBoardCCW ();
		}
	}
	
	public Vector2 getCursorLocation ()
	{
		return cursor.transform.position;
	} 
}