

/*
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

	public enum PlayerState {IDLE, WALKING, RUNNING, FROZEN};

	public PlayerState state = PlayerState.IDLE;
	public Animator DickController;
	public SpriteRenderer DickSprite;
	public TextBoxManager textBox;
	public GameObject mainCamera;
	public AudioSource footsteps;

	public bool lighterOn;
	public SpriteRenderer fieldOfVision;

	public float dx; //the player's movement direction along the x axis
	public float dy; //the player's movement direction along the y axis.
	public float xSpeed = 0.04F; //horizontal movement speed for walking
	public int runSpeed = 2; //multiplied speed when running

	public bool facingSign = false; //if an object can be inspected
	List<SignTrigger> signs;
	//public SignTrigger currentSign;
	public bool facingDoor = false; //if the player can open a door
	public string doorDest; //when the door leads

	bool newState = true;

	// Use this for initialization
	void Start () {
		switch (SceneManager.GetActiveScene ().name) {
		case "Master Bedroom":
			if (GameController.controller.prevRoom == "Closet") {
				transform.position = new Vector2 (17.74f, transform.position.y);
			}
			break;
		case "Closet":
			if (GameController.controller.prevRoom == "Nursery") {
				transform.position = new Vector2 (16.43f, transform.position.y);
				mainCamera.SendMessage ("ForceMove", new float[]{ 16, 0 });
			}
			break;
		}

		footsteps = GetComponent<AudioSource> ();
		DickController = GetComponent<Animator> ();
		DickSprite = GetComponent <SpriteRenderer> ();
		signs = new List<SignTrigger> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameController.controller.litRoom) {
		case GameController.LightState.NOLIGHT:
			fieldOfVision.enabled = true;
			lighterOn = false;
			break;
		case GameController.LightState.ROOMLIGHT:
			lighterOn = false;
			break;
		case GameController.LightState.PLAYERLIGHT:
			fieldOfVision.enabled = true;
			lighterOn = true;
			break;
		case GameController.LightState.TWOLIGHTS:
			lighterOn = true;
			break;
		}

		if (lighterOn) {
			if (fieldOfVision.transform.localScale.x <= 0.5f) {
				fieldOfVision.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
			} else if(fieldOfVision.transform.localScale.x > 0.5f){
				if (GameController.controller.candles [SceneManager.GetActiveScene ().buildIndex]) {
					fieldOfVision.enabled = false;
				}
			}
		} else {
			if (fieldOfVision.transform.localScale.x >= 0.08) {
				fieldOfVision.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			}
		}
		dx = Input.GetAxis ("Horizontal"); //0 if the player is still. positive if moving right, negative if moving left
		dy = Input.GetAxis ("Vertical"); // 0 when the player is standing still, positive to enter a door. 

		//player sprite changes direction when player walks in different directions
		if (dx < 0) {
			DickSprite.flipX = true; 
		} else if(dx > 0) {
			DickSprite.flipX = false;
		}

		switch (state) {
		case PlayerState.IDLE:
			stateIdle ();
			break;
		case PlayerState.WALKING:
			stateWalking ();
			break;
		case PlayerState.RUNNING:
			stateRunning ();
			break;
		case PlayerState.FROZEN:
			stateFrozen ();
			break;
		}
	}

	void stateIdle(){
		if (lighterOn) {
			DickController.Play ("DickIdleLighter");
		} else {
			DickController.Play ("DickIdle");
		}

		if (Input.GetKeyDown ("x")) {
			switch (GameController.controller.litRoom) {
			case GameController.LightState.NOLIGHT:
				GameController.controller.litRoom = GameController.LightState.PLAYERLIGHT;
				break;
			case GameController.LightState.ROOMLIGHT:
				GameController.controller.litRoom = GameController.LightState.TWOLIGHTS;
				break;
			case GameController.LightState.PLAYERLIGHT:
				GameController.controller.litRoom = GameController.LightState.NOLIGHT;
				break;
			case GameController.LightState.TWOLIGHTS:
				GameController.controller.litRoom = GameController.LightState.ROOMLIGHT;
				break;
			} 
		}

		//code to enter doors and change rooms. only able to enter doors when not running. 
		if (facingDoor && dy > 0) {
			GameController.controller.prevRoom = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene (doorDest);
		}

		if (facingSign && Input.GetKeyDown ("return")) {
			SignTrigger sign = signs[0];
			textBox.EnableTextBox (sign.flavorText [sign.currentLine]);
			sign.increment();
		}

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
			state = PlayerState.WALKING;
			return;
		}
	}

	void stateWalking(){
		footsteps.volume = 0.1f;

		if (lighterOn) {
			DickController.Play ("DickWalkingLighter");
		} else {
			DickController.Play ("DickWalking");
		}

		if (GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("DickStep") && newState) {
			footsteps.Play ();
			newState = false;
		} else if (GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("DickIdle")) {
			newState = true;
		}

		transform.Translate(dx*xSpeed,0,0);

		if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
			state = PlayerState.IDLE;
			return;
		}

		if (Input.GetKey (KeyCode.Z)) {
			state = PlayerState.RUNNING;
			return;
		}
	}

	void stateRunning(){
		footsteps.volume = 0.15f;

		if (lighterOn) {
			DickController.Play ("DickRunningLighter");
		} else {
			DickController.Play ("DickRunning");
		}

		if (GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("DickStep") && newState) {
			footsteps.Play ();
			newState = false;
		} else if (GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("DickIdle")) {
			newState = true;
		}

		transform.Translate(dx*xSpeed*runSpeed,0,0);

		if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
			state = PlayerState.IDLE;
			return;
		}

		if (!Input.GetKey (KeyCode.Z)) {
			state = PlayerState.WALKING;
			return;
		}
	}

	void stateFrozen(){
		if (lighterOn) {
			DickController.Play ("DickIdleLighter");
		} else {
			DickController.Play ("DickIdle");
		}

		if (facingSign && Input.GetKeyDown ("return")) {
			textBox.DisableTextBox ();
		}
	}

	//player can't move. caused by monster or other eldritch thing
	void FreezeInTerror(){
		state = PlayerState.FROZEN;
	}
	//player can move again
	void RegainControl(){
		state = PlayerState.IDLE;
	}

	//player comes in contact with an inspectable object
	//gets flavor text information from that object, already parsed. 
	void TriggerSign(SignTrigger sign){
		facingSign = true;
		signs.Insert (0,sign);
	}
	//player is no longer in contact with an inspectable object
	void unTriggerSign(SignTrigger sign){
		signs.Remove(sign);
		if (signs.Count == 0) {
			facingSign = false;
		}
	}

	//player comes in contact with a door. gets destination information from the door. 
	void TriggerDoor(string dest){
		facingDoor = true;
		doorDest = dest;
	}
	//player is no longer in contact with a door. 
	void unTriggerDoor(){
		facingDoor = false;
	}
}
*/