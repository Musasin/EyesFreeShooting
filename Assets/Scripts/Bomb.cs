using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	float scale;

	// Use this for initialization
	void Start () {
		scale = 0;
	}

	// Update is called once per frame
	void Update () {
		if (scale > 30)
			Destroy (gameObject);

		scale += (scale / 2 + 0.0001f);
		transform.localScale = new Vector3(scale, scale, 1);

	}
}
