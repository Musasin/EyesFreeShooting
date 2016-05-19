using UnityEngine;
using System.Collections;

public class EnemyFactory : MonoBehaviour {

	[SerializeField]GameObject defaultEnemy;
	int level = 1;
	float time = 0;
	float chargeTime = 0;
	ScoreBoard scoreBoard;

	// Use this for initialization
	void Start () {
		scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoard> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		chargeTime += Time.deltaTime;
		
		level = scoreBoard.getLevel ();
		if (chargeTime > (5 - Mathf.Min (level / 3, 4))) {
			chargeTime = 0;
			InstantiateEnemy();
		}

		if(GameObject.FindWithTag("Enemy") == null)
			InstantiateEnemy();
	}

	void InstantiateEnemy()
	{
		GameObject enemy = (GameObject)Instantiate (defaultEnemy);
		enemy.transform.parent = gameObject.transform;
		
		int rand = Random.Range (0, 5);
		int maxHp = 5 + level;
		Enemy.MoveType moveType;
		
		if (rand % 3 == 0)
			moveType = Enemy.MoveType.LeftRight;
		else if (rand % 3 == 1)
			moveType = Enemy.MoveType.Stop;
		else
			moveType = Enemy.MoveType.Rotation;
		
		Enemy.ShotType shotType;
		switch (rand) {
		case 0:
			shotType = Enemy.ShotType.Nothing;
			break;
		case 1:
			shotType = Enemy.ShotType.Shooting;
			break;
		case 2:
			shotType = Enemy.ShotType.Straight;
			break;
		case 3:
			shotType = Enemy.ShotType.Rotation;
			break;
		case 4:
			shotType = Enemy.ShotType.EvenShot;
			break;
		default:
			shotType = Enemy.ShotType.Nothing;
			break;
		}
		
		float shotInterval = 2 + (level / 5);
		float bulletSpead = 60 + (level * 5);
		int point = level * 100;
		
		enemy.GetComponent<Enemy> ().AllSetting (Random.Range (-2.7f, 2.7f), 
		                                         Random.Range (4.0f, 7.0f), 
		                                         maxHp, 
		                                         moveType, 
		                                         Random.Range (-6.0f, 4.0f), 
		                                         shotType, 
		                                         shotInterval,
		                                         bulletSpead,
		                                         point);
	}
}
