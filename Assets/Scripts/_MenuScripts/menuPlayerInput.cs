using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
public class menuPlayerInput : MonoBehaviour
{

	public KeyCode up, down, left, right, select;								//Keys for special inputs
	public GameObject cursor;													//Pointer to cursor on player's board
		
	void Start ()
	{	}

	void Update ()
	{
		if (Input.GetKey(left) && Input.GetKey(up)) { 
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x-5, cursor.transform.position.y+5));
		}
		else if (Input.GetKey(left) && Input.GetKey(down)) { 
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x-5, cursor.transform.position.y-5));
		}
		else if (Input.GetKey(right) && Input.GetKey(up)) { 
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x+5, cursor.transform.position.y+5));
		}
		else if (Input.GetKey(right) && Input.GetKey(down)) { 
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x+5, cursor.transform.position.y-5));
		}
		else if (Input.GetKey(left)) { 
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x-5, cursor.transform.position.y));
		} 
		//Move Cursor Right
		else if (Input.GetKey(right)){
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x+5, cursor.transform.position.y));
		} 
		//Move Cursor Up
		else if (Input.GetKey(up)) {
			cursor.transform.positionTo(.033f, new Vector2 (cursor.transform.position.x, cursor.transform.position.y+5));
		} 
		//Move Cursor Down
		else if (Input.GetKey(down)) {
			cursor.transform.positionTo (.033f, new Vector2 (cursor.transform.position.x, cursor.transform.position.y - 5));
		}
	}

	//Return screen position of player's cursor
	public Vector2 getCursorLocation ()
	{
		return cursor.transform.position;
	} 
}