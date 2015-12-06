using UnityEngine;
using System.Collections;

public class ButtonNum : MonoBehaviour {
	
	public Color tint;
	public GameObject cursor;
	public Sprite[] sprNum;
	public int num;

	void Start(){
		if (gameObject.name.ToLower () == "hundred") {
			num = 1;
		} else {
			num = 0;
		}
	}

	void Update(){
		var hit = cursor.GetComponent<cursorInput>().getCursorPosition ();
		
		var a = this.gameObject.GetComponent<SpriteRenderer> ();
		if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
			a.color = tint;
			if(Input.GetKeyDown(cursor.GetComponent<cursorInput>().select)){
				if (num < 9){
					num++;
				}
				else{
					num = 0;
				}
				a.sprite = sprNum[num];
			}
		} else {
			a.color = Color.white;
		}
	}

	public int getNum(){
		return num;
	}
}

