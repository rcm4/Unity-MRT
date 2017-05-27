using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerpz : MonoBehaviour {

    public float rotationSpeed = 0.75f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(Vector3.up * rotationSpeed);
	}
}
