using UnityEngine;
using System.Collections;

//simple timer script to control events. 
public class Timer : MonoBehaviour {

	public float startTime; // the amount of time, in seconds, that the timer counts down. 
	public float currTime;
	public bool counting = false;
	public bool alarm = false; //activated when Animator alarm reaches 0.

	//start the the timer with the time you want in seconds. 
	public void startTimer(float start){
		startTime = start;
		currTime = startTime;
		counting = true;
	}

	//let's you reuse the timer.
	public void reset(){
		alarm = false;
	}

	// if the timer is active, then every frame, the timer subtracts the time that passed over that frame. 
	void Update () {
		if (counting) {
			if (currTime > 0f) {
				currTime -= Time.deltaTime;
			}
			if (currTime <= 0) {
				counting = false;
				alarm = true;
			}
		}
	}
}