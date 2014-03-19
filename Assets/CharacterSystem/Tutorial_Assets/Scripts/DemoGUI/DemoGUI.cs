using UnityEngine;
using System.Collections;

public class DemoGUI : MonoBehaviour {
	bool showGUI = true;
	void OnGUI ()
	{
		showGUI = GUI.Toggle(new Rect(10,10,400,20),showGUI,"Show input settings");
		if (showGUI)
		{
			GUI.Label(new Rect(10,50,400,20), "INPUT v.3:");
			GUI.Label(new Rect(20,80,400,Screen.height), 
				"  1  : First-Person Camera" 
				+"\n"+"  2  : Orbit-Person Camera" 
				+"\n"+"  3  : Third-Person Camera"
				+"\n"+"  Mouse Wheel  : Camera distance"
				+"\n"+""
				+"\n"+"  W  : Forward & Double-Tap Evade"
				+"\n"+"  A  : Left & Double-Tap Evade"
				+"\n"+"  S  : Backward & Double-Tap Evade"
				+"\n"+"  D  : Right & Double-Tap Evade"
				+"\n"+""
				+"\n"+"  E  : Climb & End ragdoll manually"
				+"\n"+"  X  : Walk & Jog toggle"
				+"\n"+"  V  : Sneak toggle"
				+"\n"+""
				+"\n"+"  Q  : Cycle through assigned weapons"
				+"\n"+"  C  : Draw & Holster current weapon"
				+"\n"+"  R  : Reload (Pistol & Rifle)"
				+"\n"+""
				+"\n"+"  T  : Throw"
				+"\n"+"  G  : Dance 1"
				+"\n"+"  H  : Dance 2"
				+"\n"+"  J  : Dance 3"
				+"\n"+"  B  : Pull lever"
				+"\n"+"  N  : Push button"
				+"\n"+""
				+"\n"+"  Space  : Jump"
				+"\n"+"  Left Shift  : Sprint toggle"
				+"\n"+""
				+"\n"+"  Left Mouse  : Left attack combo chain & Shoot"
				+"\n"+"  Right Mouse  : Right attack combo chain");
			if (GUI.Button(new Rect(20,Screen.height-60,100,20), "Demo Scene"))
			{
				Application.LoadLevel(0);
			}
			else if (GUI.Button(new Rect(20,Screen.height-40,100,20), "Tutorial Scene"))
			{
				Application.LoadLevel(1);
			}
		}
	}
}
