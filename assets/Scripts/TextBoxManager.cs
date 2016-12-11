using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//this script allows for the creation and displaying of text boxes, such as for dialogue or flavor text. 
public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text signage;
	public PlayerControls player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerControls> ();
		DisableTextBox ();
	}

	//freezes the player in place and displays the selected text
	public void EnableTextBox(string signText){
		signage.text = signText;
		textBox.SetActive (true);
		player.SendMessage("FreezeInTerror");
	}

	//deactivates the text box and gives the player free movement again. 
	public void DisableTextBox(){
		textBox.SetActive (false);
		player.SendMessage ("RegainControl");
	}
}