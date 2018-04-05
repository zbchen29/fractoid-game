using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSpin : MonoBehaviour {

    Transform selfTransform;

    Vector3 nextRotationCoordinates;
    Quaternion nextRotation;

    public float turnSpeed;

	// Use this for initialization
	void Start () {

        selfTransform = gameObject.GetComponent<Transform>();

        nextRotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update () {

        nextRotationCoordinates.z += (Time.deltaTime * turnSpeed) % 360;

        nextRotation.eulerAngles = nextRotationCoordinates;

        selfTransform.rotation = nextRotation;
	}
}
