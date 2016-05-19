using UnityEngine;
using System.Collections;

public class TitleSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToTutorial()
	{
		Application.LoadLevel("tutorial");
	}
	
	public void GoToMainGame()
	{
		Application.LoadLevel("main");
	}
}
