﻿using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour {

	public Animator[,] charAnim;
	private Color charColor;
	private GameObject sprite;
	private Quaternion playRot;
	public Transform parent;

	void Awake () 
	{
		playRot = new Quaternion(0,0,0,0);
	}

	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public Characters()	{}

	public void setChar(int c,int location)
	{
		if (parent.position.x > 0) 
		{	playRot = new Quaternion(0,180,0,0);}


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
	}

	public void setMage()
	{
		setColor( new Color (1.0f, 0.196f, 0.196f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Assassin"),parent.transform.position,playRot) as GameObject;
	}

	public void setWarrior()
	{
		setColor( new Color (0.706f, 0.196f, 1.0f, 0.745f));
		sprite = Instantiate(Resources.Load("Prefabs/Characters/Assassin"),parent.transform.position,playRot) as GameObject;
	}

	public void setBlank()
	{
		setColor( new Color (0.0f, 0.0f, 0.0f, 0.0f));
	}

	public void setLocation(int loc)
	{

		sprite.transform.parent = parent;
		sprite.transform.position = new Vector3 ((sprite.transform.position.x-(3*(-1+loc))),(sprite.transform.position.y-5.6f),(sprite.transform.position.z));
	}
}