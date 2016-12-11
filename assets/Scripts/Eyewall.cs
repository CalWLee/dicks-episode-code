using UnityEngine;
using System.Collections;

public class Eyewall : MonoBehaviour {

	public enum EyewallState {OPEN, CLOSED};

	public EyewallState state;
	public Animator EyeController;

	Timer blinkTimer;
	bool newState = false;

	int startEye;
	float freq;
	int rot;

	// Use this for initialization
	void Start () {
		startEye = Random.Range (0, 2);
		freq = Random.Range (0.5f, 4.0f);
		rot = Random.Range (0, 4);

		state = (EyewallState)startEye;
		gameObject.transform.Rotate(new Vector3(0,0,90*rot));

		EyeController = GetComponent<Animator> ();
		blinkTimer = GetComponent<Timer> ();

		blinkTimer.startTimer (freq);
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case EyewallState.OPEN:
			stateOpen ();
			break;
		case EyewallState.CLOSED:
			stateClosed ();
			break;
		}
	}

	void stateClosed(){
		if (newState) {
			EyeController.Play ("EyewallBlink");
			newState = false;
		}

		if (blinkTimer.alarm) {
			blinkTimer.reset ();
			blinkTimer.startTimer (freq);
			state = EyewallState.OPEN;
			newState = true;
			return;
		}
	}

	void stateOpen(){
		if (newState) {
			EyeController.Play ("EyewallWink");
			newState = false;
		}

		if (blinkTimer.alarm) {
			blinkTimer.reset ();
			blinkTimer.startTimer (freq);
			state = EyewallState.CLOSED;
			newState = true;
			return;
		}
	}
}
