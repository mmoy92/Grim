using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public GameObject[] enemies;
	public int leftBound = -50;
	public int rightBound = 50;
	public int topBound = 50;
	public int bottomBound = -50;
	public int startDepth = -20;
	public int endDepth = 100;
	public bool spawn = false;

	// Use this for initialization
	void Start () {
		if (spawn)
			Invoke ("Spawn", .05f);
	}

	void Spawn () {
		for (int i = leftBound; i < rightBound; i += 10) {
			for (int j = bottomBound; j < topBound; j += 10) {
				for (int z = startDepth; z < endDepth; z += 10) {
					if (z > -10 && z < 10)
						continue;
					int enemyIndex = Random.Range(0, enemies.Length);
					GameObject enemy = (GameObject) Instantiate(enemies[enemyIndex], new Vector3(i, j, z), transform.rotation);
					float scale = .8f;
					enemy.transform.localScale = new Vector3(scale,scale,scale);
				}
			}
		}
	}
}
