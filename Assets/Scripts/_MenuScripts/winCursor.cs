using UnityEngine;
using System.Collections;

public class winCursor : MonoBehaviour {

	public KeyCode up, dn, lt, rt, select;
	public GameObject cursor;
	public Color tint;
		
		// Use this for initialization
		void Start () {		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKey(up) && cursor.transform.position.y < 10) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x,cursor.transform.position.y+0.4f));
			}
			if (Input.GetKey (dn) && cursor.transform.position.y > -10) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x,cursor.transform.position.y-0.4f));
			}
			if (Input.GetKey (lt) && cursor.transform.position.x > -17.5f) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x-0.4f,cursor.transform.position.y));
			}
			if (Input.GetKey (rt) && cursor.transform.position.x < 17.5f) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x+0.4f,cursor.transform.position.y));
			}
		}
		
		public RaycastHit2D getCursorPosition(){
			Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursor.transform.position));
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			return hit;
		}
}
