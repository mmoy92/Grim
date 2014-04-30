using UnityEngine;
using System.Collections;

public class PlayerHealthBarScript : MonoBehaviour
{
	
		public GUIStyle health_empty;
		public GUIStyle health_full;
		public GUIStyle key_empty;
		public GUIStyle key_full;
		public GUIStyle morality_meter;
		public GUIStyle morality_icon;
		//private GUIStyle extra_unlocked;
	
		//current progress
		public float healthBar;
		public float keyProgress;
		public float moralityBar;

		Vector2 healthPosition;
		Vector2 healthSize;

		Vector2 keyPosition;
		Vector2 keySize;

		Vector2 moralityPosition;
		Vector2 moralitySize;

		Vector2 iconPosition;
		Vector2 iconSize;

		public Texture2D iconTex;
		public Texture2D moralityMeter;
		public Texture2D emptyHealth;
		public Texture2D fullHealth;
		public Texture2D emptyKey;
		public Texture2D fullKey;
		//public Texture2D unlockTex;
 

		void Start ()
		{
				float sizeScalingX = (455f / 1366f) * Screen.width;
				float sizeScalingY = (65f / 597f) * Screen.height;

				healthPosition = new Vector2 (0, 0);
				healthSize = new Vector2 (sizeScalingX, sizeScalingY);

				keyPosition = new Vector2 (Screen.width - sizeScalingX, 0);
				keySize = new Vector2 (sizeScalingX, sizeScalingY);

				moralitySize = new Vector2 (sizeScalingX, sizeScalingY);
				moralityPosition = new Vector2 ((Screen.width - moralitySize.x)/2, Screen.height - moralitySize.y);
	
				iconSize = new Vector2 (sizeScalingX / 45.5f, sizeScalingY);
				moralityBar = 0.5f;

		}

		void OnGUI ()
		{
				//HEALTH STUFF
				GUI.BeginGroup (new Rect (healthPosition.x, healthPosition.y, healthSize.x, healthSize.y));
					GUI.Box (new Rect (0, 0, healthSize.x, healthSize.y), emptyHealth, health_empty);
					GUI.BeginGroup (new Rect (0, 0, healthSize.x * healthBar, healthSize.y));
						GUI.Box (new Rect (0, 0, healthSize.x, healthSize.y), fullHealth, health_full);
					GUI.EndGroup ();
				GUI.EndGroup ();

				//KEY STUFF	
				GUI.BeginGroup (new Rect (keyPosition.x, keyPosition.y, keySize.x, keySize.y));
					GUI.Box (new Rect (0, 0, keySize.x, keySize.y), emptyKey, key_empty);
					int xProg = (int)(keySize.x * keyProgress);
					GUI.BeginGroup (new Rect (keySize.x -xProg, 0, xProg, keySize.y));
						GUI.Box (new Rect (-keySize.x + xProg, 0, keySize.x, keySize.y), fullKey, key_full);
					GUI.EndGroup ();
				GUI.EndGroup ();

				//MORALITY STUFF
				GUI.BeginGroup (new Rect (moralityPosition.x, moralityPosition.y, moralitySize.x, moralitySize.y));
					GUI.Box (new Rect (0, 0, moralitySize.x, moralitySize.y), moralityMeter, morality_meter);
				GUI.EndGroup ();

				GUI.BeginGroup (new Rect (moralityPosition.x + 0.1219f * moralitySize.x, moralityPosition.y, (0.7582f * moralitySize.x) + iconSize.x , moralitySize.y));
		GUI.Box (new Rect ((0.7582f * moralitySize.x) * moralityBar, 0, iconSize.x, iconSize.y), iconTex, morality_icon);
				GUI.EndGroup ();

				//DASH STUFF

				//ULTIMATE STUFF
		}
	
		void Update ()
		{
				if (grimInfo.grimHP == 5) {
						healthBar = 1;
				} else if (grimInfo.grimHP == 4) {
						healthBar = 356f / 455f;
				} else if (grimInfo.grimHP == 3) {
						healthBar = 253f / 455f;
				} else if (grimInfo.grimHP == 2) {
						healthBar = 151f / 455f;
				} else if (grimInfo.grimHP == 1) {
						healthBar = 47f / 455f;
				} else
						healthBar = 0;

				keyProgress = grimInfo.keys / grimInfo.maxKeys;
		}	
}