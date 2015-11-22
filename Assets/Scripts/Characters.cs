using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour 
{
	public Color charColor;
	public GameObject sprite;
	public GameObject casing1,casing2;
	public GameObject gem1,gem2;
	private Quaternion playRot;
	public Transform parent;
	public Sprite BG;
	private int slide;

	void Awake () 
	{ 
		playRot = new Quaternion(0,0,0,0);
		if (parent.position.x > 0) 
		{	playRot = new Quaternion(0,180,0,0);}
		slide = 50;
	}

	public void setChar(int c,int location)
	{

		if (c == 1) {
			setAssassin();
		} else if (c == 2) {
			setMage();
		} else if (c == 3) {
			setWarrior();
		} else {
			setBlank();
		}

		setLocation (location);
		var a = gem1.GetComponent<Renderer> ();
		var b = gem2.GetComponent<Renderer> ();
		a.material.SetColor ("_Color", new Color(a.material.color.r,a.material.color.g,a.material.color.b,0.5f));
		b.material.SetColor ("_Color", new Color(a.material.color.r,a.material.color.g,a.material.color.b,0.5f));

		this.GetComponent<Renderer> ().material.SetColor("_Color", charColor);
	}

	private void setColor(Color c)
	{
		charColor = c;
	}

	private void setAssassin()
	{
		setColor( new Color (0.118f, 1.0f , 0.0f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Assassin"),parent.transform.position,playRot) as GameObject;

		casing1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingGold"),parent.transform.position,playRot) as GameObject;
		casing2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingSilver"),parent.transform.position,playRot) as GameObject;
		gem1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/yellow"),parent.transform.position,playRot) as GameObject;
		gem2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/green"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Assassin", typeof(Sprite)) as Sprite;
	}

	private void setMage()
	{
		setColor( new Color (0.47f, 0.0f, 0.77f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Mage"),parent.transform.position,playRot) as GameObject;
		casing1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingGold"),parent.transform.position,playRot) as GameObject;
		casing2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingSilver"),parent.transform.position,playRot) as GameObject;
		gem1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/blue"),parent.transform.position,playRot) as GameObject;
		gem2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/purple"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Mage", typeof(Sprite)) as Sprite;
	}

	private void setWarrior()
	{
		setColor( new Color (0.96f, 0.196f, 0.0f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Warrior"),parent.transform.position,playRot) as GameObject;	
		casing1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingGold"),parent.transform.position,playRot) as GameObject;
		casing2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/casingSilver"),parent.transform.position,playRot) as GameObject;
		gem1 = Instantiate(Resources.Load("Prefabs/Characters/Gems/orange"),parent.transform.position,playRot) as GameObject;
		gem2 = Instantiate(Resources.Load("Prefabs/Characters/Gems/red"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Warrior", typeof(Sprite)) as Sprite;
	}

	private void setBlank()
	{
		setColor( new Color (0.0f, 0.0f, 0.0f, 0.0f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;
		casing1 = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;
		casing2 = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;
		gem1 = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;
		gem2 = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;

		BG = Resources.Load("Backgrounds/Assassin", typeof(Sprite)) as Sprite;
	}

	private void setLocation(int loc)
	{
		sprite.transform.parent = parent;

		casing1.transform.parent = sprite.transform;
		casing2.transform.parent = sprite.transform;
		gem1.transform.parent = sprite.transform;
		gem2.transform.parent = sprite.transform;
		sprite.transform.position = new Vector3 ((sprite.transform.position.x-(3*(-1+loc))),(sprite.transform.position.y-5.6f),(sprite.transform.position.z));
		casing1.transform.position = new Vector3 ((sprite.transform.position.x-1.15f),(sprite.transform.position.y-1.15f),(sprite.transform.position.z));
		casing2.transform.position = new Vector3 ((sprite.transform.position.x+1.15f),(sprite.transform.position.y-1.15f),(sprite.transform.position.z));
		gem1.transform.position = new Vector3 ((sprite.transform.position.x-1.15f),(sprite.transform.position.y-1.15f),(sprite.transform.position.z));
		gem2.transform.position = new Vector3 ((sprite.transform.position.x+1.15f),(sprite.transform.position.y-1.15f),(sprite.transform.position.z));
	}

	public void animatetheGems(int g1,int g2)
	{
		animateGem (gem1, g1);
		animateGem (gem2, g2);
	}

	private void animateGem(GameObject gem,int gemCount)
	{
		bool gcDown = (slide >= 100);
		if (!gcDown) {
			if (gemCount < 5) {
				slide += 1;
			} else if (gemCount >= 5 && gemCount < 25) {
				slide += 2;
			} else if (gemCount >= 25 && gemCount < 30) {
				slide += 4;
			} else if (gemCount >= 30 && gemCount < 40) {
				slide += 6;
			} else if (gemCount >= 40 && gemCount < 50) {
				slide += 8;
			}
		} else if (gcDown) {
			if (gemCount < 5) {
				slide += -1;
			} else if (gemCount >= 5 && gemCount < 25) {
				slide += -2;
			} else if (gemCount >= 25 && gemCount < 30) {
				slide += -4;
			} else if (gemCount >= 30 && gemCount < 40) {
				slide += -6;
			} else if (gemCount >= 40 && gemCount < 50) {
				slide += -8;
			}
		}
		var a = gem1.GetComponent<Renderer> ();
		if (gemCount < 50) {
			a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, ((float)((50+slide)/100))));
		} else {
			a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, 1.0f));
		}
	}

}