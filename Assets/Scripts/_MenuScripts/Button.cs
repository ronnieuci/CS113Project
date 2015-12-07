using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public menuScript ms;
	public Color tint;
	public GameObject cursor;
	public int curr,next;

	void Start(){	}

	void Update(){
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();

		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
				if(gameObject.name != "quit")
					ms.loadScreen(curr,next);
				else
					Application.Quit();

			}
		} else {
			a.color = Color.white;
		}
	}
}

