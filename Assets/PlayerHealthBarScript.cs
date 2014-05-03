using UnityEngine;
using System.Collections;

public class PlayerHealthBarScript : MonoBehaviour
{
		private GameObject player;                      // Reference to the player
		private grimInfo griminfo;
		public GUIStyle health_empty;
		public GUIStyle health_full;
		public GUIStyle key_empty;
		public GUIStyle key_full;
		public GUIStyle morality_meter;
		public GUIStyle morality_icon;
		public GUIStyle dash_empty;
		public GUIStyle dash_full;
		public GUIStyle ultimate_empty;
		public GUIStyle ultimate_full;
	//private GUIStyle extra_unlocked;
	
		//current progress
		float healthBar;
		float keyProgress;
		public float moralityBar;
		float dashingTime;
		float ultimateTime;
	

		Vector2 healthPosition;
		Vector2 healthSize;
		Vector2 keyPosition;
		Vector2 keySize;
		Vector2 moralityPosition;
		Vector2 moralitySize;
		Vector2 iconPosition;
		Vector2 iconSize;
		Vector2 dashPosition;
		Vector2 dashSize;
		Vector2 ultimatePosition;
		Vector2 ultimateSize;
	
	
		public Texture2D iconTex;
		public Texture2D moralityMeter;
		public Texture2D emptyHealth;
		public Texture2D fullHealth;
		public Texture2D emptyKey;
		public Texture2D fullKey;
		public Texture2D upDash;
		public Texture2D downDash;
		public Texture2D upUltimate;
		public Texture2D downUltimate;

		private float xSize;
		private float ySize;
	private float animationSmoother;

		void Start ()
		{
				player = GameObject.FindGameObjectWithTag ("Player");
				griminfo = player.GetComponent<grimInfo> ();

				xSize = 456f;
				ySize = 66f;

				healthPosition = new Vector2 (0, 0);
				healthSize = new Vector2 (xSize, ySize);

				keyPosition = new Vector2 (Screen.width - xSize, 0);
				keySize = new Vector2 (xSize, ySize);

				moralitySize = new Vector2 (xSize, ySize);
				moralityPosition = new Vector2 ((Screen.width - xSize) / 2, Screen.height - ySize);
	
				iconSize = new Vector2 (xSize / 45f, ySize);
				moralityBar = 0.5f;

				dashPosition = new Vector2 (0, ySize);
				dashSize = new Vector2 (xSize / 2f, ySize/2f); 

				ultimatePosition = new Vector2 (xSize/2f, ySize);
<<<<<<< HEAD
<<<<<<< HEAD
				ultimateSize = new Vector2 (xSize / 2f, ySize /2f); 
=======
				ultimateSize = new Vector2 (xSize / 2f, ySize/2f); 
>>>>>>> d8184193a17831355d51eb8bdf48939cf57b38e4
=======
				ultimateSize = new Vector2 (xSize / 2f, ySize/2f); 
>>>>>>> d8184193a17831355d51eb8bdf48939cf57b38e4
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
				GUI.BeginGroup (new Rect (keySize.x - xProg, 0, xProg, keySize.y));
				GUI.Box (new Rect (-keySize.x + xProg, 0, keySize.x, keySize.y), fullKey, key_full);
				GUI.EndGroup ();
				GUI.EndGroup ();

				//MORALITY STUFF
				GUI.BeginGroup (new Rect (moralityPosition.x, moralityPosition.y, moralitySize.x, moralitySize.y));
				GUI.Box (new Rect (0, 0, moralitySize.x, moralitySize.y), moralityMeter, morality_meter);
				GUI.EndGroup ();

				GUI.BeginGroup (new Rect (moralityPosition.x + 0.1219f * moralitySize.x, moralityPosition.y, (0.7582f * moralitySize.x) + iconSize.x, moralitySize.y));
				GUI.Box (new Rect ((0.7582f * moralitySize.x) * moralityBar, 0, iconSize.x, iconSize.y), iconTex, morality_icon);
				GUI.EndGroup ();

				//DASH STUFF
				GUI.BeginGroup (new Rect (dashPosition.x, dashPosition.y, dashSize.x, dashSize.y));
				GUI.Box (new Rect (0, 0, dashSize.x, dashSize.y), downDash, dash_empty);
				GUI.BeginGroup (new Rect (0, 0, dashSize.x * dashingTime, dashSize.y));
				GUI.Box (new Rect (0, 0, dashSize.x, dashSize.y), upDash, dash_full);
				GUI.EndGroup ();
				GUI.EndGroup ();

				//ULTIMATE STUFF
			GUI.BeginGroup (new Rect (ultimatePosition.x, ultimatePosition.y, ultimateSize.x, ultimateSize.y));
			GUI.Box (new Rect (0, 0, ultimateSize.x, ultimateSize.y), downUltimate, ultimate_empty);
			GUI.BeginGroup (new Rect (0, 0, ultimateSize.x * ultimateTime, ultimateSize.y));
			GUI.Box (new Rect (0, 0, ultimateSize.x, ultimateSize.y), upUltimate, ultimate_full);
			GUI.EndGroup ();
			GUI.EndGroup ();
	}
	
	void Update ()
		{
				dashingTime = (player.GetComponent<PlatformerController> ().dashingTime)/5.0f;
				ultimateTime = (player.GetComponent<PlayerCombat> ().ultimateTimer)/5.0f;
	
				if (griminfo.grimHP == 5) {
						healthBar = 1;
				} else if (griminfo.grimHP == 4) {
						healthBar = (356f / 456f) * (456 / xSize);
				} else if (griminfo.grimHP == 3) {
						healthBar = (253f / 456f) * (456 / xSize);
				} else if (griminfo.grimHP == 2) {
						healthBar = (151f / 456f) * (456 / xSize);
				} else if (griminfo.grimHP == 1) {
						healthBar = (47f / 456f) * (456 / xSize);
				} else
						healthBar = 0;

				keyProgress = griminfo.keys / griminfo.maxKeys;
				
				if (moralityBar <= 0.0f)
						moralityBar = 0.0f;
				else if (moralityBar >= 1.0f)
						moralityBar = 1.0f;
				else
						moralityBar = 0.5f - griminfo.soulCount / 50f;
		}	
}