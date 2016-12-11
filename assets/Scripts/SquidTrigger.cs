using UnityEngine;
using System.Collections;

//This script is used to activate the squid event in the nursery
public class SquidTrigger : MonoBehaviour {

	void Update(){
		if (GameController.controller.squidSpawnedEvent) {
			Destroy (gameObject);
		}
	}

	//when the player reaches the end of the nursery, the trigger in the window the spawn the squid becomes active. 
	void OnTriggerEnter2D(Collider2D otherObject){
		GameController.controller.windowSquidEvent = true;
	}
}