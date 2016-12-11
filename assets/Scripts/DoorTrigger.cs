using UnityEngine;
using System.Collections;

//this script tells the player if the player is standing in front of a door, for the purposes of travelling between rooms.
public class DoorTrigger : MonoBehaviour {

	//When the player steps in front of a door, the door sends a message to the player telling the player where the door leads.
	//The object this script is attached to MUST be given the name of its destination.
	void OnTriggerEnter2D(Collider2D otherObject){
		otherObject.gameObject.SendMessage("TriggerDoor", gameObject.name);
	}

	void OnTriggerExit2D(Collider2D otherObject){
		otherObject.gameObject.SendMessage ("unTriggerDoor");
	}
}