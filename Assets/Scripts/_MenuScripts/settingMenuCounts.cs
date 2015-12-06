using UnityEngine;
using System.Collections;

public class settingMenuCounts : MonoBehaviour {

	public menuScript ms;
	public ButtonNum h,t,o;
	public GameObject volFill, pOnOff;
	public Color selColor;
	public int volNum;
	public Sprite[] spr;

	void Start(){
		volNum = 50;
	}

	// Update is called once per frame
	void Update () {
		showVolume ();
		showPower ();
	}

	private void showVolume(){
		var a = volFill.GetComponent<SpriteRenderer> ();
		a.transform.localScale = new Vector3 (((float)volNum / 100),((float)volNum / 100),1);
	}

	private void showPower() {
		var b = pOnOff.GetComponent<SpriteRenderer> ();
		if (ms.powers)
			b.sprite = spr [1];
		else {
			b.sprite = spr[0];
		}		              
	}

	public int getPoints(){
		return ((h.getNum()*100)+(t.getNum()*10)+(o.getNum()));
	}
}
