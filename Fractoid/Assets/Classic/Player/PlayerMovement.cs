using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Player speed
    public float speed;
    Rigidbody2D playerRB;

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKey("w"))
        {
            //transform.Translate(Vector2.up * speed * Time.deltaTime, Space.World);
            playerRB.AddForce(Vector2.up * speed);
        }
        if (Input.GetKey("a"))
        {
            //transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
            playerRB.AddForce(Vector2.left * speed);
        }
        if (Input.GetKey("s"))
        {
            //transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
            playerRB.AddForce(Vector2.down * speed);
        }
        if (Input.GetKey("d"))
        {
            //transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
            playerRB.AddForce(Vector2.right * speed);
        }
    }
}
