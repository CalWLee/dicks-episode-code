using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {
	public string playerName;

	public bool lighterOn = false;

	//Animations are saved as strings because AnimationController.Play() takes in a string.
	public string idleAnimation;
	public string walkAnimation;
	public string runAnimation;

	//Portraits
	public Sprite portrait0;
	public Sprite portrait1;
	public Sprite portrait2;
	public Sprite portrait3;
	public Sprite portrait4;
	public Sprite portrait5;
	public Sprite portrait6;
	public Sprite portrait7;
	public Sprite portrait8;
	public Sprite portrait9;

	//other attributes

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
