using UnityEngine;
using System.Collections;

public class defaultControls  {

	public KeyCode up,down,left,right, back, move, clear, shiftcw, shiftccw, swap, select, attack1, attack2, pause;


	public void setControls(int p){
		var platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer) {
			if (p == 1) {
				up = KeyCode.Joystick1Button5;
				down = KeyCode.Joystick1Button6;
				left = KeyCode.Joystick1Button7;
				right = KeyCode.Joystick1Button8;
				back = KeyCode.Joystick1Button10;
				move = KeyCode.Joystick1Button0;
				clear = KeyCode.Joystick1Button17;
				shiftcw = KeyCode.Joystick1Button14;
				shiftccw = KeyCode.Joystick1Button13;
				swap = KeyCode.Joystick1Button16;
				attack1 = KeyCode.Joystick1Button18;
				attack2 = KeyCode.Joystick1Button19;
				pause = KeyCode.Joystick1Button0;
			} else {
				up = KeyCode.Joystick2Button5;
				down = KeyCode.Joystick2Button6;
				left = KeyCode.Joystick2Button7;
				right = KeyCode.Joystick2Button8;
				back = KeyCode.Joystick2Button10;
				move = KeyCode.Joystick2Button0;
				clear = KeyCode.Joystick2Button17;
				shiftcw = KeyCode.Joystick2Button14;
				shiftccw = KeyCode.Joystick2Button13;
				swap = KeyCode.Joystick2Button16;
				attack1 = KeyCode.Joystick2Button18;
				attack2 = KeyCode.Joystick2Button19;
				pause = KeyCode.Joystick2Button0;
			}
		} else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer) {
			if (p == 1) {
				move = KeyCode.Joystick1Button0;
				clear = KeyCode.Joystick1Button1;
				shiftcw = KeyCode.Joystick1Button5;
				shiftccw = KeyCode.Joystick1Button4;
				swap = KeyCode.Joystick1Button0;
				attack1 = KeyCode.Joystick1Button2;
				attack2 = KeyCode.Joystick1Button3;
				pause = KeyCode.Joystick1Button7;
			} else {
				move = KeyCode.Joystick2Button0;
				clear = KeyCode.Joystick2Button1;
				shiftcw = KeyCode.Joystick2Button5;
				shiftccw = KeyCode.Joystick2Button4;
				swap = KeyCode.Joystick2Button0;
				attack1 = KeyCode.Joystick2Button2;
				attack2 = KeyCode.Joystick2Button3;
				pause = KeyCode.Joystick2Button7;
			}
		}
	}
}
