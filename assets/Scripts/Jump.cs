using UnityEngine;
using System.Collections;

//This script is outdated. The jumping code is now inside the player controls script. 
//maybe delete? not sure. 
public class Jump : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))  //makes player jump
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0,10), ForceMode2D.Impulse);
        }
    }
}