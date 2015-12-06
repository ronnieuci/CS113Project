using UnityEngine;
using System.Collections;

public class gameStartButton : MonoBehaviour {

	public menuScript ms;
	public Color tint;
	public GameObject cursor;
	public int curr,next;

	// Update is called once per frame
	void Update () {
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();
		
		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;

			if (Input.GetKeyDown (cursor.GetComponent<cursorInput> ().select)) {
				ms.StartGame ();
			}
		} else {
			a.color = Color.white;
		}
	}
}