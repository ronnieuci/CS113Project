using UnityEngine;
using System.Collections;

public class ButtonStart : MonoBehaviour {

	public menuScript ms;
	public Color tint;
	public GameObject cursor;
	public int curr,next;
	
	
	void Start(){	}
	
	void Update(){
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();
		
		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (ms.ready) {
			if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
				a.color = tint;
				if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
					ms.loadScreen(curr,next);
				}
			} else {
				a.color = Color.white;
		}

		} else {
			a.color = new Color(1,1,1,0.33f);
		}
	}
}


