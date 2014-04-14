using UnityEngine;
using System.Collections;

public class BGSoundScript : MonoBehaviour {
	public bool mute1 = true;
	public bool mute2 = true;
	public bool mute3 = true;
	public bool mute4 = true;

	private AudioSource audio1;
	private AudioSource audio2;
	private AudioSource audio3;
	private AudioSource audio4;

	// Use this for initialization
	void Start () {
		AudioSource[] audios = GetComponents<AudioSource>();
		audio1 = audios[0];
		audio2 = audios[1];
		audio3 = audios[2];
		audio4 = audios[3];

		audio1.Play();
		audio2.Play();
		audio3.Play();
		audio4.Play();
	}
	
	// Update is called once per frame
	void Update () {
		audio1.mute = mute1;
		audio2.mute = mute2;
		audio3.mute = mute3;
		audio4.mute = mute4;
	}
}
