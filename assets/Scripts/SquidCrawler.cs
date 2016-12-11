using UnityEngine;
using System.Collections;

//This script holds the behavior for the squid monster. Currently both forms of the squid are separate object, but they both use the 
//same behavior script. the only difference is that when they transform, the method "transform" of the first form is called,
//which in turn calls the method "morph" inside of the second form. Then the bahavior stays the same as before. This will be changed 
//when I learn more about how animation works so I can make the transformation into an animation. 
public class SquidCrawler : MonoBehaviour {


	public enum SquidState {DESPAWNED, SPAWNED, CRAWLING, TELEPORTING, TRANSFORMED, WALKING, DEFEATED};

	public SquidState state = SquidState.DESPAWNED;
	Timer waitTime;
	public float crawlSpeed = 0.01f;
	public float walkSpeed = 0.03f;
	public GameObject player;
	//used to make the screen fade to black. 
	public SpriteRenderer fade;
	public SpriteRenderer flash;
	public Camera mainCamera;
	public Animator SquidController;
	public WindowTrigger trigger;

	public bool newState = true;

	void Start () {
		waitTime = GetComponent<Timer> ();
		SquidController = GetComponent<Animator> ();
	}

	void Update(){

		if (state != SquidState.DESPAWNED && newState &&
		GameController.controller.litRoom == GameController.LightState.TWOLIGHTS) {
			GameController.controller.squidLighterEvent = true;

			state = SquidState.DEFEATED;
		}

		switch (state) {
		case SquidState.DESPAWNED:
			stateDespawned ();
			break;
		case SquidState.SPAWNED:
			stateSpawned ();
			break;
		case SquidState.CRAWLING:
			stateCrawling ();
			break;
		case SquidState.TELEPORTING:
			stateTeleporting ();
			break;
		case SquidState.TRANSFORMED:
			stateTransformed ();
			break;
		case SquidState.WALKING:
			stateWalking ();
			break;
		case SquidState.DEFEATED:
			stateDefeated ();
			break;
		}
	}

	void stateDespawned(){
		if (GameController.controller.litRoom != GameController.LightState.TWOLIGHTS &&
		   trigger.squidExists && player.transform.position.x > transform.position.x) {
			Spawn ();
		}
	}

	void stateSpawned(){
		SquidController.Play ("SquidCrawling");
		if (waitTime.alarm) {
			waitTime.reset ();
			state = SquidState.CRAWLING;
		}
	}

	void stateCrawling(){
		if (GetComponent<SpriteRenderer> ().sprite.name == "SQUID B") {
			transform.Translate (crawlSpeed * 4, 0, 0);
		}
	}

	void stateTeleporting(){
		if (waitTime.counting && waitTime.currTime <= 0.5) {
			if (GameController.controller.squidLighterEvent) {
				flash.color = new Color (1, 1, 1, (2f * waitTime.currTime));
			}

			if (GameController.controller.squidDisappearEvent) {
				fade.color = new Color (0, 0, 0, (2f * waitTime.currTime));
			}
		}

		if (waitTime.alarm) {
			waitTime.reset ();
			fade.enabled = false;
			flash.enabled = false;
			player.SendMessage ("RegainControl");
			Destroy (gameObject);
		}
	}

	void stateTransformed(){
		SquidController.Play ("SquidWalking");

		if (waitTime.counting && waitTime.currTime <= 0.5) {
			flash.color = new Color (1, 1, 1, (2f * waitTime.currTime));
		}

		if (waitTime.alarm) {
			waitTime.reset ();
			flash.enabled = false;
			flash.color = Color.white;
			player.SendMessage ("RegainControl");
			waitTime.startTimer (0.5f);
			state = SquidState.WALKING;
		}
	}

	void stateWalking(){
		if (waitTime.alarm) {
			GetComponent<Rigidbody2D> ().gravityScale = 1;
			GetComponent<BoxCollider2D> ().enabled = true;
			transform.Translate (walkSpeed, 0, 0);
		}
	}
		
	void stateDefeated(){
		if (newState) {
			newState = false;

			//the camera fades
			flash.enabled = true;
			waitTime.reset ();
			waitTime.startTimer (2.0f); //determines how long the camera stays faded out.

			//the player is teleported to the room entrance. the squid teleports offscreen.
			player.SendMessage ("FreezeInTerror");
			transform.position = new Vector2 (100, 100);
		}
		//The squid stops moving, and tells the player to also stop moving.
		state = SquidState.TELEPORTING;
	}

	//When the squid is triggered, it becomes visible and its hitbox becomes active. 
	void Spawn(){
		if (GameController.controller.litRoom != GameController.LightState.TWOLIGHTS) {
			GameController.controller.squidSpawnedEvent = true;

			GetComponent<Rigidbody2D> ().gravityScale = 1;
			GetComponent<SpriteRenderer> ().enabled = true;
			GetComponent<BoxCollider2D> ().enabled = true;
			GameController.controller.sanity--;

			//the squid stands still for a moment before crawling. This timer measures how long it stands still.
			waitTime.startTimer (1f);
			state = SquidState.SPAWNED;
		}
	}

	//called when the squid reaches the moonlight. turns off the first form of the squid and calld "morph" on the second form. 
	void Transform(){
		GameController.controller.squidMoonlightEvent = true;

		GetComponent<Rigidbody2D> ().gravityScale = 0;
		GetComponent<BoxCollider2D> ().enabled = false;
		flash.enabled = true;
		waitTime.startTimer (2f); //determines how long the camera stays faded out.

		//the player is teleported to the room entrance. the squid teleports offscreen.
		player.SendMessage ("FreezeInTerror");
		GetComponent<BoxCollider2D> ().offset = new Vector2 (-0.03233961f, -0.09841897f);
		GetComponent<BoxCollider2D> ().size = new Vector2 (0.2179753f, 0.3432364f); 
		GameController.controller.sanity--;
		transform.position = new Vector3 (7.21f, -1.187664f, 0);
		transform.localScale += new Vector3(3,3,0); 
		state = SquidState.TRANSFORMED;
	}

	//Activated when the squid collides with the player.
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.name == "Dick") {
			GameController.controller.squidDisappearEvent = true;

			waitTime.reset ();
			//the camera fades
			fade.enabled = true;
			waitTime.startTimer (2f); //determines how long the camera stays faded out.

			//the player is teleported to the room entrance. the squid teleports offscreen.
			player.SendMessage ("FreezeInTerror");
			GameController.controller.sanity--;
			transform.position = new Vector2 (100, 100);
			player.transform.position = new Vector2 (-10.93f, -1.04f);
			mainCamera.SendMessage ("ForceMove", new float[]{ -4f, 0.71f });

			//The squid stops moving, and tells the player to also stop moving.
			state = SquidState.TELEPORTING;
		}
	}

	/*
	public float crawlSpeed = 0f; 
	public bool stopCrawling = false;
	// holds a reference to the transformed version of the squid. Will probably beome obsolete when I learn 
	// more about how to animate. 
	public GameObject form; 
	//used to make the screen fade to black. 
	public SpriteRenderer fade;
	public Camera mainCamera;

	GameObject player;
	Timer waitTime;

	void Start () {
		waitTime = GetComponent<Timer> ();
	}

	void Update () {
		//After the squid is spawned, when the timer ends, it begins to crawl towards the player. 
		if (waitTime.alarm && !stopCrawling) {
			crawlSpeed = 0.02f;
			waitTime.reset ();
		}

		//after the player and the squid collide, and the camera fades back in, the player can move again and the squid destroys itself.
		if (waitTime.alarm && stopCrawling) {
			fade.enabled = false;
			player.SendMessage ("RegainControl");
			Destroy (gameObject);
		}

		//as long as the squid has not been stopped, it will continue to crawl towards the player. 
		if (!stopCrawling) {
			transform.Translate (crawlSpeed, 0, 0);
		}
	}

	//When the squid is triggered, it becomes visible and its hitbox becomes active. 
	void Spawn(){
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;
		GameController.controller.sanity--;

		//the squid stands still for a moment before crawling. This timer measures how long it stands still.
		waitTime.startTimer (1f);
	}

	//called when the squid reaches the moonlight. turns off the first form of the squid and calld "morph" on the second form. 
	void Transform(){
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		form.SendMessage ("Morph");
	}

	//activates the second form of the squid. the second forms's sprite and hitbox are turned on and it begins crawling towards the 
	//player even faster. 
	void Morph(){
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;
		GameController.controller.sanity--;
		stopCrawling = false;
		crawlSpeed = 0.03f;
	}

	//Activated when the squid collides with the player.
	void OnCollisionEnter2D(Collision2D collision){
		//The squid stops moving, and tells the player to also stop moving.
		crawlSpeed = 0f;
		stopCrawling = true;
		collision.gameObject.SendMessage ("FreezeInTerror");

		//the camera fades
		fade.enabled = true;
		waitTime.startTimer (2f); //determines how long the camera stays faded out. 

		//the player is teleported to the room entrance. the squid teleports offscreen.
		GameController.controller.sanity--;
		transform.position = new Vector2 (100, 100);
		collision.transform.position = new Vector2 (-10.93f, -1.04f);
		mainCamera.SendMessage ("ForceMove",new float[]{0,0});

		//keeps a reference to the player.
		player = collision.gameObject;
	}
	*/
}