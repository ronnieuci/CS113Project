using UnityEngine;
using System.Collections;

public class cursorInput : MonoBehaviour {

	public KeyCode up, dn, lt, rt, select;
	public GameObject cursor, chsprite;
	public RuntimeAnimatorController[] sprites;
	public Color tint;
	public int character;

	// Use this for initialization
	void Start () {
		character = -1;
		tint = Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(up) && cursor.transform.position.y < 9) {
			cursor.transform.positionTo (0.01f, new Vector2(cursor.transform.position.x,cursor.transform.position.y+0.25f));
		}
		if (Input.GetKey (dn) && cursor.transform.position.y > -9) {
			cursor.transform.positionTo (0.01f, new Vector2(cursor.transform.position.x,cursor.transform.position.y-0.25f));
		}
		if (Input.GetKey (lt) && cursor.transform.position.x > -13) {
			cursor.transform.positionTo (0.01f, new Vector2(cursor.transform.position.x-0.25f,cursor.transform.position.y));
		}
		if (Input.GetKey (rt) && cursor.transform.position.x < 14) {
			cursor.transform.positionTo (0.01f, new Vector2(cursor.transform.position.x+0.25f,cursor.transform.position.y));
		}

		showChar (character);
	}

	public RaycastHit2D getCursorPosition(){
		Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursor.transform.position));
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
		return hit;
	}

	public void showChar(int c)
	{
		var a = chsprite.GetComponent<Animator>();
		if (c >= 0){
			a.runtimeAnimatorController = sprites[c];
		}
	}
}
