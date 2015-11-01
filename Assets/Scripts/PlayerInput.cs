using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	//Keys
	public KeyCode up,down,left,right,move,clear,shiftcw,shiftccw,swap,temp;
	public ShapesManager sm;
	public float x,y,tempnum;
	public GameObject cursor;
	private string direction;
	
	void Awake(){}
	
	void Start(){}

	void Update () 
	{
		if (Input.GetKeyDown (left) )	 {if (x>0){ cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x-1,cursor.transform.localPosition.y,0)); x-=1;}}
		if (Input.GetKeyDown (right))	 {if (x<6){ cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x+1,cursor.transform.localPosition.y,0)); x+=1;}}
		if (Input.GetKeyDown (up)) 		 {if (y<7){ cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y+1,0)); y+=1;}}
		if (Input.GetKeyDown (down)) 	 {if (y>0){ cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y-1,0)); y-=1;}}
		if (Input.GetKeyDown (clear)) 	 {sm.ResetBoard(); }
		if (Input.GetKeyDown (shiftcw))  {sm.rotateBoardCW();}
		if (Input.GetKeyDown (shiftccw)) {sm.rotateBoardCCW();}
	}

	
	public Vector2 getCursorLocation()
	{
		return cursor.transform.position;
	} 
}

/*
 * 		if (direction == "U") 	   {direction = "R"; sm.SetGravityCW (direction);tempnum=x;x=y;y=tempnum;cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y,0));}
		else if (direction == "R") {direction = "D"; sm.SetGravityCW (direction);tempnum=x;x=y;y=tempnum;cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y,0));}
		else if (direction == "D") {direction = "L"; sm.SetGravityCW (direction);tempnum=x;x=y;y=tempnum;cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y,0));}
		else if (direction == "L") {direction = "U"; sm.SetGravityCW (direction);tempnum=x;x=y;y=tempnum;cursor.transform.localPositionTo(Constants.MoveAnimationMinDuration,new Vector3(cursor.transform.localPosition.x,cursor.transform.localPosition.y,0));}
		cursor.transform.Rotate (new Vector3 (0, 0, 90));
		temp=up; up=right; right=down; down=left; left=temp;
*/