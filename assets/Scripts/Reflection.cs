using UnityEngine;
using System.Collections;

//this script controls the player's reflection in the closet. 
public class Reflection : MonoBehaviour {

	int q; //the offset of the reflection from the player. Currently set to 2. 
	public PlayerControls original; // The player being relflected. 
	float reflectDx; //the direction in which the relfection is moving. Should match the player's. 
	string state;

	Animator refController;

	void Start () {
		q = 2;

		refController = GetComponent<Animator> ();

		//The reflection is always moving with the player, but is only visible while the player is in front of the mirror. 
		transform.position = new Vector2 (original.transform.position.x+q, original.transform.position.y);
	}

	void Update () {
		//the reflection moves when the player does, but does not walk, since only its top half is visible. 
		transform.position = new Vector2 (original.transform.position.x+q, original.transform.position.y);
		reflectDx = original.dx;

		//the reflection changes the direction its facing when it moves in a different direction. 
		if (reflectDx < 0) {
			GetComponent <SpriteRenderer> ().flipX = true;
		} else if(reflectDx > 0) {
			GetComponent <SpriteRenderer> ().flipX = false;
		}
			
		//the reflection bounces when the player does.
		switch (original.state) {
		case PlayerControls.PlayerState.IDLE:
			if (original.character.lighterOn) {
				refController.Play ("DickIdleLighter");
			} else {
				refController.Play ("DickIdle");
			}
			break;
		case PlayerControls.PlayerState.RUNNING:
			if (original.character.lighterOn) {
				refController.Play ("DickRunningLighter");
			} else {
				refController.Play ("DickRunning");
			}
			break;
		case PlayerControls.PlayerState.WALKING:
			if (original.character.lighterOn) {
				refController.Play ("DickWalkingLighter");
			} else {
				refController.Play ("DickWalking");
			}
			break;
		}
	}
}