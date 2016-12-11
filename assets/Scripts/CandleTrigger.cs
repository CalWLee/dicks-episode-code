using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CandleTrigger : MonoBehaviour {
	public Animator candleController;
	public PlayerControls Dick = null;
	public SpriteRenderer candleLight;
	SignTrigger sign;
	int room;

	void Start(){
		candleController = GetComponent<Animator> ();
		room = SceneManager.GetActiveScene ().buildIndex;
		sign = GetComponent<SignTrigger> ();
	}

	void OnTriggerEnter2D(Collider2D otherObject){
		Dick = otherObject.GetComponent<PlayerControls> ();
	}

	void OnTriggerExit2D(Collider2D otherObject){
		Dick = null;
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameController.controller.litRoom) {
		case GameController.LightState.NOLIGHT:
			candleLight.enabled = false;
			break;
		case GameController.LightState.PLAYERLIGHT:
			candleLight.enabled = false;
			if (Dick != null) {
				GameController.controller.candles [room] = true;
				GameController.controller.litRoom = GameController.LightState.TWOLIGHTS;
			}
			break;
		case GameController.LightState.TWOLIGHTS:
			candleController.Play ("CandleLit");
			if (candleLight.transform.localScale.x <= 1.0) {
				candleLight.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
			} else {
				candleLight.enabled = false;
			}
			sign.flavorText [0] = "Now that there's more light in here I can finally see";
			sign.flavorText [1] = "This candle is the light of my life.";
			break;
		case GameController.LightState.ROOMLIGHT:
			candleController.Play ("CandleLit");
			candleLight.enabled = true;
			if (candleLight.transform.localScale.x > 0.4618734) {
				candleLight.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			}
			sign.flavorText [0] = "Now that there's more light in here I can finally see";
			sign.flavorText [1] = "This candle is the light of my life.";
			break;
		}
	}
}