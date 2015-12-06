using UnityEngine;
using System.Collections;

public class winCursor : MonoBehaviour {
	public GameObject cursor;
	public Color tint;
	public KeyCode select;

	private Vector3 move;
	private CharacterController characterController;
	private defaultControls dc = new defaultControls();
	private int player;

		
		// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		player = PlayerPrefs.GetInt ("winner");
		dc.setControls (player);
		select = dc.swap;
	}
		
		// Update is called once per frame
		void Update () {

		move.x = 0;
		move.y = 0;
		
		
		if(cursor.transform.position.y > -17.5f && cursor.transform.position.y < 17.5f)
			move.x = Input.GetAxis ("L_XAxis_1") * 20;
		if(cursor.transform.position.y < 10 && cursor.transform.position.y > -10)
			move.y = Input.GetAxis ("L_YAxis_1") * -20;

		if (Input.GetKey(dc.up) && cursor.transform.position.y < 10) {
			cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x,cursor.transform.position.y+0.4f));
			}
		if (Input.GetKey (dc.down) && cursor.transform.position.y > -10) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x,cursor.transform.position.y-0.4f));
			}
		if (Input.GetKey (dc.left) && cursor.transform.position.x > -17.5f) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x-0.4f,cursor.transform.position.y));
			}
		if (Input.GetKey (dc.right) && cursor.transform.position.x < 17.5f) {
				cursor.transform.positionTo (0.005f, new Vector2(cursor.transform.position.x+0.4f,cursor.transform.position.y));
			}
		}
		
		public RaycastHit2D getCursorPosition(){
			Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursor.transform.position));
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			return hit;
		}
}
