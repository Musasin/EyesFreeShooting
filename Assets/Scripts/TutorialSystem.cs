using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour {
	
	Text tutorialText;
	
	string nowText = "";

	int stringNumber;
	float elapsedTime;
	const float ADD_TEXT_INTERVAL = 0.05f;
	TouchSystem touchSystem;
	[SerializeField] GameObject tutorialEnemy;
	GameObject enemy;
	GameObject player;
	GameObject blindPanel;

	enum Mode{
		START,
		SWIPE,
		ENEMY1,
		ENEMY2,
		ENEMY3,
		ENEMY_SHOT,
		ENEMY_SHOT2,
		PLAYER_SHOT,
		PLAYER_SHOT2,
		BOMB1,
		BOMB2,
		BOMB3,
		BOMB4,
		BLIND_SHOT,
		END
	};
	Mode nowMode = Mode.START;

	// Use this for initialization
	void Start () {
		nowText = "チュートリアルの世界へようこそ！\nこちらでは操作方法と、音の紹介を行います。";
		tutorialText = GameObject.Find("TutorialText").GetComponent<Text> ();
		touchSystem = GetComponent<TouchSystem>();
		blindPanel = GameObject.Find ("blindPanel");
		blindPanel.SetActive (false);
		player = GameObject.Find ("player");
		player.GetComponent<Player> ().setCanShot (false);
		player.GetComponent<Player> ().setCanBomb (false);
		tutorialText.text = "";
		elapsedTime = 0;
		stringNumber = 0;
	}

	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;

		if (elapsedTime > ADD_TEXT_INTERVAL && stringNumber < nowText.Length) {
			tutorialText.text += nowText[stringNumber];
			elapsedTime = 0;
			stringNumber++;
		}

		switch (nowMode) {
		case Mode.START:
			if(canGoNext(4))
			{
				resetText("まずは移動をしてみましょう。\nお好きな場所をタッチして、そのままなぞってみてください。");
				nowMode = Mode.SWIPE;
			}
			break;
		case Mode.SWIPE:
			if(canGoNext(1) && touchSystem.isSwiping())
			{
				resetText("そう。ばっちりです。\n次に、敵をお見せします。");
				nowMode = Mode.ENEMY1;
				enemy = Instantiate(tutorialEnemy);
			}
			break;
		case Mode.ENEMY1:
			if(enemy.transform.position.y < 0)
			{
				resetText("こちらが敵です。\n敵には直接触れてもダメージはありません。");
				nowMode = Mode.ENEMY2;
			}
			break;
		case Mode.ENEMY2:
			if(canGoNext(3) && touchSystem.isSwiping())
			{
				resetText("また、この音が敵の位置を知る重要な情報となります。\n色々な位置から確認してみましょう。");
				nowMode = Mode.ENEMY3;
			}
			break;
		case Mode.ENEMY3:
			if(canGoNext(5))
			{
				enemy.GetComponent<Enemy>().ChangeType(Enemy.MoveType.Stop, Enemy.ShotType.Straight);
				resetText("敵の撃つ弾に当たると残機を失います。\nその時、残り何人かを音声でお知らせします。");
				nowMode = Mode.ENEMY_SHOT;
			}
			break;
		case Mode.ENEMY_SHOT:
			if(canGoNext(5))
			{
				resetText("弾の音が近づいてきたら逆方向に逃げられるように心がけましょう。");
				nowMode = Mode.ENEMY_SHOT2;
			}
			break;
		case Mode.ENEMY_SHOT2:
			if(canGoNext(5))
			{
				player.GetComponent<Player> ().setCanShot (true);
				resetText("それでは反撃をしてみましょう。\nこちらの攻撃は自動で行われます。");
				nowMode = Mode.PLAYER_SHOT;
			}
			break;
		case Mode.PLAYER_SHOT:
			if(canGoNext(3))
			{
				resetText("敵の弾をかわしながら攻撃して、\n敵を倒してください。");
				nowMode = Mode.PLAYER_SHOT2;
			}
			break;
		case Mode.PLAYER_SHOT2:
			if(enemy == null)
			{
				resetText("お見事。");
				nowMode = Mode.BOMB1;
			}
			break;
		case Mode.BOMB1:
			if(canGoNext(3))
			{
				enemy = Instantiate(tutorialEnemy);
				enemy.GetComponent<Enemy>().ChangeType(Enemy.MoveType.LeftRight, Enemy.ShotType.Rotation);
				player.GetComponent<Player>().setCanShot(false);
				player.GetComponent<Player>().setCanBomb(true);
				resetText("次に、ボムについて紹介します。\nなぞって移動している最中に別の指でタッチすることで発動します。");
				nowMode = Mode.BOMB2;
			}
			break;
		case Mode.BOMB2:
			if(canGoNext(6))
			{
				resetText("言い換えると、二本指でタッチすることで発動します。\n周囲の弾を全て消し去る事ができます。");
				nowMode = Mode.BOMB3;
			}
			break;
		case Mode.BOMB3:
			if(GameObject.FindWithTag("EnemyBullet") == false)
			{
				Destroy(enemy);
				resetText("素晴らしい。\nそれでは最後に、暗闇の中の敵を倒してみてください。");
				nowMode = Mode.BOMB4;
			}
			break;
		case Mode.BOMB4:
			if(canGoNext (3))
			{
				enemy = Instantiate(tutorialEnemy);
				enemy.GetComponent<Enemy>().ChangeType(Enemy.MoveType.LeftRight, Enemy.ShotType.Shooting);
				enemy.GetComponent<Enemy>().SetHP(15);
				player.GetComponent<Player>().setCanShot(true);
				resetText("音を頼りに敵を追いかけて攻撃を当てましょう。\n敵の弾にお気をつけて。");
				blindPanel.SetActive (true);
				nowMode = Mode.BLIND_SHOT;
			}
			break;
		case Mode.BLIND_SHOT:
			if(enemy == null)
			{
				resetText("お見事です。\nこれにてチュートリアルを終了します。");
				nowMode = Mode.END;
			}
			break;
		case Mode.END:
			if(canGoNext(6))
			{
				Application.LoadLevel("title");
			}
			break;
		}
	}

	bool canGoNext(float standbyTime)
	{
		return elapsedTime > standbyTime && stringNumber >= nowText.Length;
	}
	void resetText(string newText)
	{
		tutorialText.text = "";
		stringNumber = 0;
		nowText = newText;
	}

}
