using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField]bool isPlayerBullet;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Wall")
			Destroy (gameObject);
		if (col.transform.tag == "Enemy" && isPlayerBullet)
			Destroy (gameObject);
		if (col.transform.tag == "Player" && !isPlayerBullet)
			Destroy (gameObject);
		if (col.transform.tag == "Bomb" && !isPlayerBullet)
			Destroy (gameObject);
	}

}
