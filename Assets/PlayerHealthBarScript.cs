using UnityEngine;
using System.Collections;

public class PlayerHealthBarScript : MonoBehaviour
{
	
	public GUIStyle progress_empty;
	public GUIStyle progress_full;
	//private GUIStyle extra_unlocked;
	
	//current progress
	public float barDisplay;
	
	Vector2 pos = new Vector2(0,0);
	Vector2 size = new Vector2(250,75);
	
	public Texture2D emptyTex;
	public Texture2D fullTex;
	//public Texture2D unlockTex;

	void OnGUI()
	{
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y), emptyTex, progress_empty);
		
		//GUI.Box(new Rect(pos.x, pos.y, size.x, size.y), fullTex, progress_full);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
		
		GUI.Box(new Rect(0, 0, size.x, size.y), fullTex, progress_full);

		GUI.EndGroup();
		GUI.EndGroup();
	}
	
	void Update()
	{
		//the player's health
		barDisplay = grimInfo.grimHP/grimInfo.maxHP;
	}
	
}