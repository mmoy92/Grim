using UnityEngine;
using System.Collections;

public class moviescript : MonoBehaviour {
	public MovieTexture movie;
	// Use this for initialization
	void Start () {
		StartCoroutine(StartMovie());
	}
	IEnumerator StartMovie() {
		while(!movie.isReadyToPlay)
			yield return null;
		renderer.material.mainTexture = movie;
		Debug.Log ("play movie");
		movie.Play();
		audio.Play();

		yield return new WaitForSeconds(36.0f);
		Debug.Log ("Load next level");
		Application.LoadLevel(1);
	}
	// Update is called once per frame
	void Update () {
		movie.Play();
	}
}
