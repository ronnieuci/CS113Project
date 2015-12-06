using UnityEngine;
using System.Collections;

public class defaultControls : MonoBehaviour {

	public KeyCode move, clear, shiftcw, shiftccw, select, attack1, attack2, pause;

	// Use this for initialization
	void Start () 
	{
		print (Application.platform);
	}
}
