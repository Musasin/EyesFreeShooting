using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	
	[SerializeField]GameObject enemyBullet;
	[SerializeField]AudioClip hitBullet;
	[SerializeField]AudioClip enemyDead;
	[SerializeField]AudioClip enemyShot;

	[SerializeField]int maxHp;
	[SerializeField]MoveType moveType;
	[SerializeField]float stopY;
	[SerializeField]ShotType shotType;
	[SerializeField]float shotInterval;
	[SerializeField]float bulletSpeed;
	[SerializeField]int point;

	ScoreBoard scoreBoard;

	AudioSource audioSource;
	int hp;
	bool isDead = false;
	bool isInline = true;//false;
	bool isRightMoving = true;
	float chargeTime = 0;
	float time = 0;
	
	public enum MoveType{
		Stop,
		LeftRight,
		Rotation
	}
	public enum ShotType{
		Nothing,
		Straight,
		Rotation,
		Shooting,
		EvenShot
	}

	// Use this for initialization
	void Start () {
		hp = maxHp;
		audioSource = GetComponent<AudioSource> ();
		scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<ScoreBoard> ();
	}
	
	// Update is called once per frame
	void Update () {
		chargeTime += Time.deltaTime;
		time += Time.deltaTime;
		
		if(!audioSource.isPlaying)
			Destroy (gameObject);

		if (CanShot())
			Shot ();
	}

	void FixedUpdate()
	{
		Move ();
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "PlayerBullet") {
			audioSource.PlayOneShot (hitBullet);
			hp -= 1;
		}
		if (col.transform.tag == "Bomb") {
			audioSource.PlayOneShot (hitBullet);
			hp -= 5;
		}
		if (hp <= 0) {
			audioSource.clip = enemyDead;
			audioSource.loop = false;
			audioSource.Play ();
			Destroy (GetComponent<Collider2D>());
			Destroy (GetComponent<Renderer>());
			isDead = true;
			scoreBoard.AddScore(point);
		}
		if (col.name == "DeleteEnemy")
			Destroy (gameObject);
		if (col.name == "StartupEnemy")
			isInline = true;
	}

	void Shot()
	{
		float degree;
		float radian;

		if (shotType == ShotType.Nothing)
			return;

		GameObject bullet = (GameObject)Instantiate (enemyBullet, transform.position, transform.rotation);

		degree = AngleSetting (shotType);
		radian = degree * Mathf.PI / 180.0f;
		bullet.transform.Rotate (new Vector3(0,0,degree));
		bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos (radian), Mathf.Sin(radian)) * bulletSpeed);
		audioSource.PlayOneShot (enemyShot);
		chargeTime = 0;
	}

	float AngleSetting(ShotType shotType)
	{	
		switch(shotType)
		{	
		case ShotType.Rotation:	return (time * 100);			
		case ShotType.Straight:	return 270;
		case ShotType.Shooting:	return GetAim (transform.position, GameObject.FindWithTag("Player").transform.position);
		case ShotType.EvenShot:	return GetAim (transform.position, GameObject.FindWithTag("Player").transform.position) + ((Random.value < 0.5f) ? 5 : -5);
		default: return 0;
		}
	}

	public float GetAim(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}


	void Move()
	{
		float moveX = 0;
		float moveY = 0;
		
		switch (moveType) {
		case MoveType.Stop:
			break;
		case MoveType.LeftRight:
			if(transform.position.x > 2) isRightMoving = false;
			if(transform.position.x < -2) isRightMoving = true;
			moveX = (isRightMoving) ? 0.04f : -0.04f;
			break;
		case MoveType.Rotation:
			float degree = (time*100);
			float radian = degree * Mathf.PI / 180.0f;
			moveX = Mathf.Cos (radian) / 20;
			moveY = Mathf.Sin(radian) / 20;
			break;
		}
		
		if (transform.localPosition.y > stopY)
			moveY -= 0.02f;

		gameObject.transform.Translate(new Vector3(moveX, moveY, 0.0f));
	}

	
	bool CanShot(){	
		return chargeTime > shotInterval && isInline && !isDead;
	}

	public void ChangeType(MoveType newMoveType, ShotType newShotType)
	{
		moveType = newMoveType;
		shotType = newShotType;
	}

	public void SetHP(int _maxHp)
	{
		maxHp = _maxHp;
		hp = _maxHp;
	}

	public void AllSetting(float _positionX, float _positionY, int _maxHp, MoveType _moveType, float _stopY, ShotType _shotType, float _shotInterval, float _bulletSpeed, int _point)
	{
		transform.Translate(_positionX, _positionY, 0);
		maxHp = _maxHp;
		hp = _maxHp;
		moveType = _moveType;
		stopY = _stopY;
		shotType = _shotType;
		shotInterval = _shotInterval;
		bulletSpeed = _bulletSpeed;
		point = _point;
	}
}
