using UnityEngine;
using System.Collections;

public class load : MonoBehaviour {

	void Start(){
		var c1 = PlayerPrefs.GetString ("char1");
		var c2 = PlayerPrefs.GetString ("char2");
		print (c1);
			print (c2);
	}
}
