using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour {

	private Color charColor;
	public GameObject sprite;
	private Quaternion playRot;
	public Transform parent;
	public Sprite BG;

	void Awake () 
	{ 
		playRot = new Quaternion(0,0,0,0);
		if (parent.position.x > 0) 
		{	playRot = new Quaternion(0,180,0,0);}
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
		this.GetComponent<Renderer> ().material.SetColor("_Color", charColor);
	}

	public void setColor(Color c)
	{
		charColor = c;
	}

	public void setAssassin()
	{
		setColor( new Color (0.118f, 1.0f , 0.0f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Assassin"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Assassin", typeof(Sprite)) as Sprite;
	}

	public void setMage()
	{
		setColor( new Color (0.47f, 0.0f, 0.77f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Mage"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Mage", typeof(Sprite)) as Sprite;
	}

	public void setWarrior()
	{
		setColor( new Color (0.96f, 0.196f, 0.0f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Warrior"),parent.transform.position,playRot) as GameObject;	
		BG = Resources.Load("Backgrounds/Warrior", typeof(Sprite)) as Sprite;
	}

	public void setBlank()
	{
		setColor( new Color (0.0f, 0.0f, 0.0f, 0.0f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Blank"),parent.transform.position,playRot) as GameObject;
		BG = Resources.Load("Backgrounds/Assassin", typeof(Sprite)) as Sprite;
	}

	public void setLocation(int loc)
	{

		sprite.transform.parent = parent;
		sprite.transform.position = new Vector3 ((sprite.transform.position.x-(3*(-1+loc))),(sprite.transform.position.y-5.6f),(sprite.transform.position.z));
	}
}