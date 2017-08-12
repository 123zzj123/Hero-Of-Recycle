﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour {
	public float maxVelocity = 50;
	//	public Vector3 playerPosition;
	public Transform player;
	private bool findThePlayer = false;
	private Animator animator;
	public characterProperty CharacterProperty;
	void Start () {
		CharacterProperty = this.gameObject.GetComponent<characterProperty>();
		animator = GetComponent<Animator> ();
	}

	void Update () {
		findPlayer ();						//if the player is close enough to track
		directionCtrl ();					//trun direction to the player
		moveAndAttack ();					//move and attack player
	}

	void FixedUpdate() {
		if (CharacterProperty.speed <= 0.0f)
			CharacterProperty.speed = 0.0f;
		if (CharacterProperty.damageValue <= 0.0f)
			CharacterProperty.damageValue = 0.0f;
		if (CharacterProperty.life <= 0) {
			animator.SetBool ("dead", true);
		}
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("hit!");
		if (collision.gameObject.name != "Bullet(Clone)")
			return;
		Debug.Log ("hit the bullet!");
		if (collision.gameObject.tag == this.gameObject.tag) {
			CharacterProperty.speed -= 0.1f;
			CharacterProperty.life -= 4;
			CharacterProperty.damageValue -= 0.5f;
		} else {
			CharacterProperty.speed += 0.1f;
			CharacterProperty.life += 4;
			CharacterProperty.damageValue += 0.5f;			
		}
	}

	void findPlayer() {
		if (CloseToTrack()) {
			findThePlayer = true;
		}
	}

	bool CloseToTrack() {
		return Vector3.Distance (transform.position, player.position) < 100;
	}

	bool closeToAttack() {
		return Vector3.Distance (transform.position, player.position) < 4;
	}

	void directionCtrl() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (!findThePlayer && currentClip != "Death")
			return;
		Vector3 targetDir = player.position - transform.position;
		float step = 10 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}


	void moveAndAttack() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;

		if (!findThePlayer || currentClip == "Death")
			return;
		Vector3 v = GetComponent<Rigidbody> ().velocity;
		if (CloseToTrack () && !closeToAttack ()) {
			if (currentClip == "Attack")
				return;
			if (currentClip != "Idle" && currentClip != "Death") {
				if (v.sqrMagnitude < maxVelocity) {
					v += CharacterProperty.speed * transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
			}
			animator.SetBool ("run", true);		
		} else if (closeToAttack ()) {
			animator.SetTrigger ("attack");
		} else {
			animator.SetBool ("idle", true);
		}
	}

	/*
	void shoot() {
		Animator animator = GetComponent<Animator> ();
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			switch(currentClip) {
			case "Idle":
				animator.SetTrigger ("standShoot");
				break;	 
			case "Run":
				animator.SetTrigger ("runShoot");
				break;
			} 
			myFactory mF = Singleton<myFactory>.Instance;
			GameObject bullet = mF.getBullet ();
			Vector3 point = c.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height/2, c.nearClipPlane));
			bullet.transform.position = point + transform.forward * 2;
			bullet.transform.forward = c.transform.forward;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward.normalized * 10, ForceMode.Impulse);
		}
	}
	*/
}
