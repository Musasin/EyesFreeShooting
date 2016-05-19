using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	[SerializeField]GameObject playerBullet;
	[SerializeField]GameObject bomb;

	AudioSource[] audioSources;
	[SerializeField]AudioClip damageClip;
	[SerializeField]AudioClip bombClip;
	[SerializeField]AudioClip bombVoiceClip;
	
	[SerializeField]AudioClip voiceAto;
	[SerializeField]AudioClip[] voiceNum;
	[SerializeField]AudioClip voiceNokori;
	[SerializeField]AudioClip[] voiceNumNin;
	[SerializeField]AudioClip voiceNin;
	
	[SerializeField]AudioClip voiceYoui;
	[SerializeField]AudioClip voiceStart;
	[SerializeField]AudioClip voiceEnd;

	float chargeTime;
	float bombChargeTime;
	float invincibleTime;
	const float invincibleLimit = 3.0f;
	const int MAX_BOMB_SYZE = 3;
	int bombNum = MAX_BOMB_SYZE;
	int life = 4;
	bool canShot = true;
	bool canBomb = true;
	bool isDead = false;

	GameObject blindPanel;
	GameObject titleButton;

	enum VoiceState{
		MUTE,
		YOUI,
		START,
		BOMB_NOKORI,
		BOMB_NUM,
		LIFE_ATO,
		LIFE_NUM,
		LIFE_NIN,
		END
	}
	VoiceState voiceState = VoiceState.YOUI;

	// Use this for initialization
	void Start () {
		chargeTime = 0;
		bombChargeTime = 2;
		audioSources = GetComponents<AudioSource> ();
		blindPanel = GameObject.Find ("BlindPanel");
		titleButton = GameObject.Find ("TitleButton");
		if(titleButton != null)
			titleButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		chargeTime += Time.deltaTime;
		bombChargeTime += Time.deltaTime;
		invincibleTime += Time.deltaTime;

		if (isInvincible()) {
			GetComponent<Renderer> ().enabled = !(GetComponent<Renderer> ().enabled);
		} else {
			GetComponent<Renderer> ().enabled = true;
		}

		if (chargeTime > 0.1f && canShot) {
			GameObject bullet = (GameObject)Instantiate (playerBullet, transform.position, transform.rotation);
			bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 250);
			chargeTime = 0;
		}

		switch (voiceState) {
		case VoiceState.MUTE:
			break;
		case VoiceState.YOUI:
			if(!audioSources[1].isPlaying)
			{
				audioSources[1].clip = voiceYoui;
				audioSources[1].Play();
				voiceState = VoiceState.START;
			}
			break;
		case VoiceState.START:
			if(!audioSources[1].isPlaying)
			{
				audioSources[1].clip = voiceStart;
				audioSources[1].Play();
				voiceState = VoiceState.MUTE;
			}
			break;

		case VoiceState.BOMB_NOKORI:
			if(bombChargeTime > 1.0f){
			audioSources[1].clip = voiceNokori;
			audioSources[1].Play();
			voiceState = VoiceState.BOMB_NUM;
			}
			break;
		case VoiceState.BOMB_NUM:
			if(!audioSources[1].isPlaying)
			{
				audioSources[1].clip = voiceNum[bombNum];
				audioSources[1].Play();
				voiceState = VoiceState.MUTE;
			}
			break;
			
		case VoiceState.LIFE_ATO:
			if(invincibleTime > 1.0f){
				audioSources[1].clip = voiceAto;
				audioSources[1].Play();
				voiceState = VoiceState.LIFE_NUM;
			}
			break;
		case VoiceState.LIFE_NUM:
			if(!audioSources[1].isPlaying)
			{
				audioSources[1].clip = voiceNumNin[life];
				audioSources[1].Play();
				if(life == 2 || life == 1)
					voiceState = VoiceState.MUTE;
				else
					voiceState = VoiceState.LIFE_NIN;
			}
			break;
		case VoiceState.LIFE_NIN:
			if(!audioSources[1].isPlaying)
			{
				audioSources[1].clip = voiceNin;
				audioSources[1].Play();
				voiceState = VoiceState.MUTE;
			}
			break;
		case VoiceState.END:
			if(invincibleTime > 1.0f){
				audioSources[1].clip = voiceEnd;
				audioSources[1].Play();
				voiceState = VoiceState.MUTE;
			}
			break;
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "EnemyBullet") {
			audioSources[0].PlayOneShot (damageClip);
			voiceState = VoiceState.LIFE_ATO;
			invincibleTime = 0;
			bombNum = MAX_BOMB_SYZE;
			life--;

			if(life <= 0)
			{
				if(Application.loadedLevelName == "tutorial")
				{
					life = 1;
					return;
				}
				audioSources[1].PlayOneShot (voiceEnd);
				audioSources[1].PlayOneShot (voiceEnd);
				audioSources[1].PlayOneShot (voiceEnd);
				blindPanel.SetActive(false);
				titleButton.SetActive(true);
				isDead = true;
				Time.timeScale = 0;
				//voiceState = VoiceState.END;
			}
		}
	}

	public bool isInvincible()
	{
		return invincibleTime < invincibleLimit;
	}

	public void invocationBomb()
	{
		if (bombChargeTime < 2 || !canBomb)
			return;

		if (bombNum <= 0) {
			voiceState = VoiceState.BOMB_NOKORI;
			return;
		}

		audioSources[0].PlayOneShot (bombClip);
		//audioSource.PlayOneShot (bombVoiceClip);
		voiceState = VoiceState.BOMB_NOKORI;
		Instantiate (bomb, transform.position, transform.rotation);
		bombChargeTime = 0;
		invincibleTime = 0;
		bombNum--;
	}
	
	public void setCanShot(bool b)
	{
		canShot = b;
	}
	public void setCanBomb(bool b)
	{
		canBomb = b;
	}

	public bool getIsDead()
	{
		return isDead;
	}
}
