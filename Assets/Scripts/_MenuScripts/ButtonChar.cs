using UnityEngine;
using System.Collections;

public class ButtonChar : MonoBehaviour {
		
	public menuScript ms;
	public Color tint;
	public GameObject cursor;
	public int charnum;

	void Start(){	

	}
		
	void Update(){
		var loc = cursor.GetComponent<cursorInput> ();
		var hit = loc.getCursorPosition ();
		var a = this.gameObject.GetComponent<SpriteRenderer> ();

		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
				loc.character = charnum;
			}
		} else if (cursor.GetComponent<cursorInput> ().character != charnum){
			a.color = Color.white;
		}
		highlight (cursor);
	}

	void highlight(GameObject c){
		if (c.GetComponent<cursorInput>().character == charnum) {
			this.GetComponent<SpriteRenderer>().color = tint;
		}
	}

}