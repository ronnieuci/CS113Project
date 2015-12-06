using UnityEngine;
using System.Collections;

public class ButtonWin : MonoBehaviour {

	public Color tint;
	public winCursor cursor;
	public winScreen ws;
	public bool another;

	void Update(){
		var hit = cursor.GetComponent<winCursor>().getCursorPosition ();
	
	var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
		a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<winCursor>().select)){
				ws.action(another);
		}
	} else {
		a.color = Color.white;
	}
	}
}