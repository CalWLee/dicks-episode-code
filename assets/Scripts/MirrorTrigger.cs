using UnityEngine;
using System.Collections;

//this script controls the mirror in the bedroom closet. 
public class MirrorTrigger : MonoBehaviour {

	public enum OozeState {MIRROR, ROOM, FILLED};

	public OozeState state = OozeState.MIRROR;

	public Timer oozeFiller; //keeps track of how much time the mirror has been filling with ooze.
	bool hasEntered = false; //true when the player enters the trigger at the end of the mirror.
	public GameObject ooze; //the ooze that fills the mirror. Currently just a black box. 
	public GameObject roomOoze;
	public GameObject player;
	public GameObject mainCamera;
	float oozeOrigin; //the y value of the ooze before it starts moving.
	float roomOozeOrigin;
	public SpriteRenderer fade;

	void Start() {
		oozeFiller = GetComponent<Timer> ();
		oozeOrigin = ooze.transform.position.x;
		roomOozeOrigin = roomOoze.transform.position.x;
	}

	//if the timer is active, the ooze slowly fills the mirror. 
	void Update () {

		switch (state) {
		case OozeState.MIRROR:
			if (oozeFiller.counting) {
				ooze.transform.position = new Vector2 (oozeOrigin - ((10 - oozeFiller.currTime) / 0.45f), ooze.transform.position.y);
			}
			if (oozeFiller.alarm) {
				GameController.controller.eyeWallEvent = true;

				oozeFiller.reset ();
				oozeFiller.startTimer (14f);
				state = OozeState.ROOM;
			}
			break;
		case OozeState.ROOM:
			if (oozeFiller.counting) {
				roomOoze.transform.position = new Vector2 (roomOozeOrigin + ((14 - oozeFiller.currTime) / 0.3f), roomOoze.transform.position.y);
			}

			if (oozeFiller.alarm) {
				GameController.controller.eyeConsumeEvent = true;

				fade.enabled = true;
				oozeFiller.reset ();
				oozeFiller.startTimer (1f);

				player.SendMessage ("FreezeInTerror");
				GameController.controller.sanity--;
				player.transform.position = new Vector2 (-10.72f, -2.06f);
				mainCamera.SendMessage ("ForceMove", new float[]{ -4, 0 });
				Destroy (ooze);
				Destroy (roomOoze);

				state = OozeState.FILLED;
			}
			break;
		case OozeState.FILLED:
			if (oozeFiller.counting && oozeFiller.currTime <= 0.5f) {
				fade.color = new Color(0,0,0, (2f * oozeFiller.currTime));
			}

			if (oozeFiller.alarm) {
				oozeFiller.reset ();
				fade.enabled = false;
				player.SendMessage ("RegainControl");
			}
			break;
		}
	}

	//When the player enters the trigger at the end of the mirror, the timer stars counting, activating the ooze. 
	void OnTriggerEnter2D(Collider2D otherObject){
		if (!hasEntered && !GameController.controller.mirrorEyeEvent) {
			hasEntered = true;
			GameController.controller.mirrorEyeEvent = true;
			oozeFiller.startTimer(10f); //the ooze takes 10 seconds to fill the mirror, but this can be changed. 
		}
	}
}