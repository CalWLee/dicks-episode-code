using UnityEngine;
using System.Collections;

//script that controls the trigger in the window of the nursery. spawns the squid and transforms it. 
public class WindowTrigger : MonoBehaviour {
	//true if the squid has been spawned
	public bool squidExists = false;
	public SpriteRenderer moonLight;

	//activated when the player or squid walks in front of the window
	void OnTriggerEnter2D(Collider2D otherObject){
		if (otherObject.name == "Dick") { //if the player walks in front of the window
			//if the player has already reached the end of the room, so the trigger is active
			//and if the squid hasn't been spawned yet (so that it doesn't respawn every time the player crosses the window)
			//spawn the squid
			if (GameController.controller.windowSquidEvent && !squidExists &&
			!GameController.controller.squidSpawnedEvent) { 
				GameObject.Find ("SQUID").SendMessage ("Spawn");
				squidExists = true;
			}
		} else if(!GameController.controller.squidMoonlightEvent){ //if the squid walks in front of the window
			//it changes form. 
			otherObject.SendMessage("Transform");
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	void Update(){
		if (GameController.controller.litRoom == GameController.LightState.TWOLIGHTS) {
			if (moonLight.transform.localScale.x <= 1.0) {
				moonLight.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
			} else {
				moonLight.enabled = false;
			}
		} else {
			moonLight.enabled = true;
			if (moonLight.transform.localScale.x > 0.3512538) {
				moonLight.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			}
		}
	}
}