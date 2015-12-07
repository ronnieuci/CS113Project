using UnityEngine;
using System.Collections;

public class ButtonPowers : MonoBehaviour {

	public menuScript ms;
	private Color tint;
	public GameObject cursor;
	public settingMenuCounts sm;

	void Start(){
		tint = sm.selColor;
	}
	
	void Update(){
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();
		
		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
				if (this.gameObject.name == "on")
					ms.powers = true;
				else
					ms.powers = false;
			}
		} else {
			a.color = Color.white;
		}
	}
}

