using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour 
{
	public Color charColor;
	public GameObject sprite,casing1,casing2,gem1,gem2;
	public GameObject[] bonus = new GameObject[2];
	public Transform parent;
	public Sprite BG;

	private Quaternion playRot;
	private int slider1,slider2;
	private bool Down1,Down2;

	void Awake () 
	{ 
		slider1 = 0;
		slider2 = 0;
		Down1 = false;
		Down2 = false;

		playRot = new Quaternion(0,0,0,0);
		if (parent.position.x > 0) 
		{	playRot = new Quaternion(0,180,0,0);  }

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
	
	public void setBlockBonus(GameObject[] a)
	{
		bonus[0] = a[0];
		bonus[1] = a[1];
	}

	public void animatetheGems(int g1,int g2)
	{
		animateGem1 (g1);
		animateGem2 (g2);
	}

	private void animateGem1(int gemCount)
	{
		if (gemCount != 0) {

			if (!Down1) {
				if (gemCount < 0) {
					slider1 = 0; 
				} else if (gemCount >= 0 && gemCount < 10) {
					slider1 += 3;
				} else if (gemCount >= 10 && gemCount < 20) {
					slider1 += 6;
				} else if (gemCount >= 20 && gemCount < 30) {
					slider1 += 12;
				} else if (gemCount >= 30 && gemCount < 40) {
					slider1 += 25;
				} else if (gemCount >= 40 && gemCount < 50) {
					slider1 += 50;
				} else {
					Down1 = true;
				}
			} else if (Down1) {
				if (gemCount < 5 && gemCount >= 0) {
					slider1 += -3;
				} else if (gemCount >= 05 && gemCount < 20) {
					slider1 += -6;
				} else if (gemCount >= 20 && gemCount < 30) {
					slider1 += -12;
				} else if (gemCount >= 30 && gemCount < 40) {
					slider1 += -25;
				} else if (gemCount >= 40 && gemCount < 50) {
					slider1 += -50;
				} else {
					Down1 = false;
				}
			}
			if (slider1 >= 500) {
				Down1 = true;
			} else if (slider1 <= 0) {
				Down1 = false;
			}

			var a = gem1.GetComponent<Renderer> ();
			if (gemCount <= 0) {
				a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, 0.4f));
			} else if (gemCount > 0 && gemCount < 50) {
				a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, ((float)((500.0f + slider1) / 1000))));
			} else {
				a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, 1.5f));
			}
		}
	}

	private void animateGem2(int gemCount)
	{
		if (gemCount != 0) {
				
				if (!Down2) {
					if (gemCount < 0) {
						slider2 = 0; 
					} else if (gemCount >= 0 && gemCount < 10) {
						slider2 += 3;
					} else if (gemCount >= 10 && gemCount < 20) {
						slider2 += 6;
					} else if (gemCount >= 20 && gemCount < 30) {
						slider2 += 12;
					} else if (gemCount >= 30 && gemCount < 40) {
						slider2 += 25;
					} else if (gemCount >= 40 && gemCount < 50) {
						slider2 += 50;
					} else {
						Down2 = true;
					}
				} else if (Down2) {
					if (gemCount < 5 && gemCount >= 0) {
						slider2 += -3;
					} else if (gemCount >= 05 && gemCount < 20) {
						slider2 += -6;
					} else if (gemCount >= 20 && gemCount < 30) {
						slider2 += -12;
					} else if (gemCount >= 30 && gemCount < 40) {
						slider2 += -25;
					} else if (gemCount >= 40 && gemCount < 50) {
						slider2 += -50;
					} else {
						Down2 = false;
					}
				}
				if (slider2 >= 500) {
					Down2 = true;
				} else if (slider2 <= 0) {
					Down2 = false;
				}
			}

		var a = gem2.GetComponent<Renderer> ();
		if (gemCount <= 0) {
			a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, 0.4f));
		}
		else if (gemCount > 0 && gemCount < 50) {
			a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, ((float)((500.0f+slider2)/1000))));
		} else {
			a.material.SetColor ("_Color", new Color (a.material.color.r, a.material.color.g, a.material.color.b, 1.5f));
		}
	}
}