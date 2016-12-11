using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//this script allows the camera to follow the player in rooms that are too wide to fit solely inside the camera. 
public class CameraMovement : MonoBehaviour {

	public Transform player; 
	public int[] walls; //holds 2 values: the beginning and end of the room. The camera will not go past these points. 
	public int offset = 0; //instead of being centered on the player, the camera follows the player on an offset. Currently set to 6. 

	void Start(){

		walls = new int[2];

		//the walls are decided based on which room the player is occupying.
		switch (SceneManager.GetActiveScene ().buildIndex) {
		case GameController.MASTER_BEDROOM:
			walls [0] = -12;
			walls [1] = 11;
			//offset = 6;
			break;
		case GameController.CLOSET:
			walls [0] = -4;
			walls [1] = 20;
			//offset = 6;
			break;
		case GameController.NURSERY:
			walls [0] = -4;
			walls [1] = 9;
			//offset = -6;
			break;
		}
	}

	void Update () 
	{	
		//This code keeps the camera following the player. 
		if(((player.position.x+offset) >= walls[0]) && ((player.position.x+offset) <= walls[1])){
			transform.position = new Vector3 (player.position.x + offset, transform.position.y, -10); 
		}
	}

	//If the player is teleported for any reason, this method needs to be called or else the camera won't follow the player. 
	//When called, the camera will automatically snap to wherever the player is in the room. 
	void ForceMove(float[] xy){
		transform.position = new Vector3 (xy[0], xy[1], -10);
	}
}