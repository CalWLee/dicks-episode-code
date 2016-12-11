using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

	public enum PlayerState {IDLE, WALKING, RUNNING, FROZEN};
	public enum Item {LIGHTER};

	public PlayerState state = PlayerState.IDLE;
	public Item[] inventory;
	public int currentItem = 0;

	public Animator animationController;
	public SpriteRenderer sprite;
	public TextBoxManager textBox;
	public AudioSource footsteps;
	public Character character;

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
		footsteps = GetComponent<AudioSource> ();
		animationController = GetComponent<Animator> ();
		sprite = GetComponent <SpriteRenderer> ();
		signs = new List<SignTrigger> ();
		character = GetComponent<Character> ();

		inventory = new Item[10];
		if (character.playerName == "Dick") {
			inventory [0] = Item.LIGHTER;
		}
	}

	// Update is called once per frame
	void Update () {			
		dx = Input.GetAxis ("Horizontal"); //0 if the player is still. positive if moving right, negative if moving left
		dy = Input.GetAxis ("Vertical"); // 0 when the player is standing still, positive to enter a door. 

		//player sprite changes direction when player walks in different directions
		if (dx < 0) {
			sprite.flipX = true; 
		} else if(dx > 0) {
			sprite.flipX = false;
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
		animationController.Play (character.idleAnimation);

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

		if (Input.GetKeyDown ("x")) {
			switch (inventory [currentItem]) {
			case Item.LIGHTER:
				character.lighterOn = !character.lighterOn;
				break;
			}
		}
	}

	void stateWalking(){
		footsteps.volume = 0.1f;

		animationController.Play (character.walkAnimation);

		if (GetComponent<SpriteRenderer> ().sprite.name.Contains ("Step") && newState) {
			footsteps.Play ();
			newState = false;
		} else if (GetComponent<SpriteRenderer> ().sprite.name.Contains ("Idle")) {
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

		animationController.Play (character.runAnimation);

		if (GetComponent<SpriteRenderer> ().sprite.name.Contains ("Step") && newState) {
			footsteps.Play ();
			newState = false;
		} else if (GetComponent<SpriteRenderer> ().sprite.name.Contains ("Idle")) {
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
		animationController.Play (character.idleAnimation);

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