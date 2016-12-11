using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This script tells the player if they are facing an object that is inspectable. 
//The object will act like a sign, but does not have to be a sign. 
public class SignTrigger : MonoBehaviour {

	public string[] flavorText = new string[2]; //The text that the player will read when they inspect the object. Holds 2 seperate lines of text. 
	public int currentLine = 0; 

	//when the player is in range of a sign, the sign tells them what their flavor text is. 
	void OnTriggerEnter2D(Collider2D otherObject){ 
		if (otherObject.name == "Dick") {
			otherObject.gameObject.SendMessage ("TriggerSign", this);
		}
	}

	void OnTriggerExit2D(Collider2D otherObject){
		if (otherObject.name == "Dick") {
			otherObject.gameObject.SendMessage ("unTriggerSign", this);
		}
	}

	public void increment(){
		if (currentLine == flavorText.Length - 1) {
			currentLine = 0;
		} else {
			currentLine++;
		}
	}
}