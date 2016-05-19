using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

	Text myText;
	int score = 0;
	int level = 1;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		int num = score;
		int digit = 0;
		while (num != 0) {
			num /= 5;
			digit++;
		}
		level = Mathf.Max(digit-2, 1);

		myText.text = "Level: " + level + "\nScore:" + score;
	}

	public void AddScore(int point)
	{
		score += point;
	}
	public int getLevel()
	{
		return level;
	}
}
