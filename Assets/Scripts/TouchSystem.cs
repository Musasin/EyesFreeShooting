using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchSystem : MonoBehaviour {
	
	int waitFrame;
	int holdFrame;

	float time;

	Vector2 cursorPos;
	Vector2 playerPos;
	GameObject player;

	enum Mode { 
		NOTHING,
		TOUCH,
		SWIPE,
		SEPARATE,
		END
	};
	Mode nowMode = Mode.NOTHING;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		nowMode = modeCheck ();
		time += Time.deltaTime;
		
		switch (nowMode) {
		case Mode.NOTHING: 	updateNothing();  break;
		case Mode.TOUCH: 	updateTouch();    break;
		case Mode.SWIPE: 	updateSwipe();    break;
		case Mode.SEPARATE: updateSeparate(); break;
		}
	}

	Mode modeCheck()
	{
		if (isTouching ()) {
			if (holdFrame == 0)	return Mode.TOUCH;
			else				return Mode.SWIPE;
		} else {
			if (holdFrame == 0)	return Mode.NOTHING;
			else				return Mode.SEPARATE;
		}
	}
	
	void addTrack(){
		if (player.GetComponent<Player> ().getIsDead ())
			return;

		Vector2 newPos = updateNowPos();
		cursorPos = newPos;

		playerPos = Camera.main.ScreenToWorldPoint (cursorPos);
		player.transform.position = playerPos;
		
	}
	
	bool isTouching(){
		return Input.GetMouseButton (0) || Input.touchCount > 0;
	}
	
	Vector3 updateNowPos()
	{
		//UpdatePos (Touch)
		if(Input.touchCount > 0)
			return Input.GetTouch(0).position;

		//UpdatePos (Mouse)
		if(Input.GetMouseButton (0))
			return Input.mousePosition;

		return new Vector3(0,0,0);
	}
	
	
	void updateNothing(){
		waitFrame++;
	}
	
	void updateTouch(){
		holdFrame++;
		waitFrame = 0;
	}
	void updateSwipe(){
		holdFrame++;
		addTrack();
		bombCheck ();
	}
	void updateSeparate(){
		holdFrame = 0;
	}

	void bombCheck()
	{
		if (player.GetComponent<Player> ().getIsDead ())
			return;
		if (Input.touchCount == 2 || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
			player.GetComponent<Player> ().invocationBomb ();
	}

	public bool isSwiping()
	{
		return nowMode == Mode.SWIPE;
	}

	
	public void BackToTitle()
	{
		Time.timeScale = 1;
		Application.LoadLevel("title");
	}
}
