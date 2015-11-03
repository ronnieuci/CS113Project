using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	
	//Keys
	public KeyCode up, down, left, right, move, clear, shiftcw, shiftccw, swap, temp;
	public ShapesManager sm;
	public float x, y, tempnum;
	public GameObject cursor;


	void Awake ()
	{}
	
	void Start ()
	{	}

	void Update ()
	{
		if (Input.GetKeyDown (left)) {
			if (cursor.transform.localPosition.x > -3) {  
				cursor.transform.localPosition += Vector3.left;
			}
		} else if (Input.GetKeyDown (right)) {
			if (cursor.transform.localPosition.x < 3) { 
				cursor.transform.localPosition += Vector3.right;
			}
		} else if (Input.GetKeyDown (up)) {
			if (cursor.transform.localPosition.y < 3) { 
				cursor.transform.localPosition += Vector3.up;
			}
		}
		else if (Input.GetKeyDown (down)) {
			if (cursor.transform.localPosition.y> -3) {
				cursor.transform.localPosition += Vector3.down;
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