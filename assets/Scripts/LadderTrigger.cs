using UnityEngine;
using System.Collections;

//The script tells the player if they are facing a ladder so that they can climb it. 
public class LadderTrigger : MonoBehaviour {

	//keeps track of how many ladders the player is facing at a time, so that if the player leaves one ladder
	//but is still facing another ladder, the player isn't told that they're not in front of a ladder. 
	static int ladderCount = 0;

	void OnTriggerEnter2D(Collider2D otherObject){
		ladderCount++;
		otherObject.gameObject.SendMessage("TriggerLadder");
	}

	void OnTriggerExit2D(Collider2D otherObject){
		ladderCount--;
		if(ladderCount <= 0){
			otherObject.gameObject.SendMessage ("unTriggerLadder");
		}
	}
}