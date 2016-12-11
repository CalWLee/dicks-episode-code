using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This script is only used for debugging purposes. Don't touch it. 
public class GoToSceneOne : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene (GameController.MASTER_BEDROOM);
	}
}