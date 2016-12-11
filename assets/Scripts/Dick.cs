using UnityEngine;
using System.Collections;

public class Dick : Character {

	void Start () {
		playerName = "Dick";
		idleAnimation = "DickIdle";
		walkAnimation = "DickWalking";
		runAnimation = "DickRunning";
	}
	
	// Update is called once per frame
	void Update () {
		if (lighterOn) {
			idleAnimation = "DickIdleLighter";
			walkAnimation = "DickWalkingLighter";
			runAnimation = "DickRunningLighter";
		} else {
			idleAnimation = "DickIdle";
			walkAnimation = "DickWalking";
			runAnimation = "DickRunning";
		}
	}
}
