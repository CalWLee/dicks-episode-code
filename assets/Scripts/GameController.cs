using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//the Game Controller is a persistant class that exists in every room of the game and holds all kinds of important information.
public class GameController : MonoBehaviour {
	public enum LightState {NOLIGHT, ROOMLIGHT, PLAYERLIGHT, TWOLIGHTS};
	private const int ROOM_COUNT = 3;

	public static GameController controller;
	public PlayerControls player;
	public Camera mainCamera;

	public const int MASTER_BEDROOM = 1;
	public const int CLOSET = 2;
	public const int NURSERY = 3;

	//These "visit" bools are false if the player has never entered a particular room, and true once they enter for the first time. 
	public bool nurseryVisit = false;
	public bool closetVisit = false;
	public bool masterVisit = false;

	public int timeElapsed;
	public int sanity;
	public string prevRoom;
	public bool[] candles;
	public LightState litRoom;

	/********flags********/
	//closet
	public bool mirrorEyeEvent = false;
	public bool eyeWallEvent = false;
	public bool eyeConsumeEvent = false;
	//nursery
	public bool windowSquidEvent = false;
	public bool squidSpawnedEvent = false;
	public bool squidMoonlightEvent = false;
	public bool squidDisappearEvent = false;
	public bool squidLighterEvent = false;

	//There should only be one instance of GameController, so if the player enters a room and there already exists a Game Controller,
	//the one that is meant to be created is destroyed, and the original stays. If this is the first room the player has entered,
	//the GameController is created. 
	void Awake () {
		if (controller == null) {
			DontDestroyOnLoad (gameObject);
			controller = this;
			candles = new bool[ROOM_COUNT+1];
		} else if (controller != this) {
			Destroy (gameObject);
		}
	}

	//when the player enters a room, this method determines if the player has visited the room before. If they have not, the boolean 
	//for that room's visit is set to true, and the time in the story is incremented 1 hour. If they have before, nothing happens. 
	void OnLevelWasLoaded(int index) {
		if (controller == this) {
			player = FindObjectOfType<PlayerControls> ();
			mainCamera = FindObjectOfType<Camera> ();
			switch (index) {
			case MASTER_BEDROOM:
				if (!masterVisit) {
					masterVisit = true;
					timeElapsed++;
				}
				if (prevRoom == "Closet") {
					player.transform.position = new Vector2 (17.74f, player.transform.position.y);
				}
				break;
			case CLOSET:
				if (!closetVisit) {
					closetVisit = true;
					timeElapsed++;
				}
				if (GameController.controller.prevRoom == "Nursery") {
					player.transform.position = new Vector2 (16.43f, player.transform.position.y);
					mainCamera.SendMessage ("ForceMove", new float[]{ 16, 0 });
				}
				break;
			case NURSERY:
				if (!nurseryVisit) {
					nurseryVisit = true;
					timeElapsed++;
				}
				break;
			}

			if (candles [index] || index == NURSERY) {
				litRoom = LightState.ROOMLIGHT;
			} else {
				litRoom = LightState.NOLIGHT;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (player != null) {
			int scene = SceneManager.GetActiveScene ().buildIndex;

			if (player.character.lighterOn) {
				if (candles [scene] || scene == NURSERY) {
					litRoom = LightState.TWOLIGHTS;
				} else {
					litRoom = LightState.PLAYERLIGHT;
					player.fieldOfVision.enabled = true;
				}
			} else {
				if (candles [scene] || scene == NURSERY) {
					litRoom = LightState.ROOMLIGHT;
				} else {
					litRoom = LightState.NOLIGHT;
					player.fieldOfVision.enabled = true;
				}
			}

			if (player.character.lighterOn) {
				if (player.fieldOfVision.transform.localScale.x <= 0.5f) {
					player.fieldOfVision.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
				} else if (player.fieldOfVision.transform.localScale.x > 0.5f) {
					if (GameController.controller.candles [SceneManager.GetActiveScene ().buildIndex]) {
						player.fieldOfVision.enabled = false;
					}
				}
			} else {
				if (player.fieldOfVision.transform.localScale.x >= 0.08) {
					player.fieldOfVision.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
				}
			}
		}
	}

	//Prints out inportant game information. Used for debugging purposes. 
	void OnGUI(){
		GUI.Label (new Rect (10, 10, 300, 30), "Time Elapsed: " + timeElapsed);
		GUI.Label (new Rect (10, 40, 300, 30), "Sanity : " + sanity);
	}
}