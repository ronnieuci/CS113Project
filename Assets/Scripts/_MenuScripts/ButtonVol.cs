using UnityEngine;
using System.Collections;

public class ButtonVol : MonoBehaviour {
	
	public GameObject cursor;
	public Color tint;
	public settingMenuCounts sm;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update(){
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();
		
		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
				if(this.gameObject.name == "up"){
					if (sm.volNum >= 0 && sm.volNum < 100)
						sm.volNum +=5;
				}
				else{
					if (sm.volNum > 0 && sm.volNum <= 100)
						sm.volNum -=5;
				}
			}
		} else {
			a.color = Color.white;
		}
	}
}
