using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uniform : MonoBehaviour {

    private GameObject MyCube;
	// Use this for initialization
	void Start () {
        MyCube = GameObject.Find("Cube");
    }
	
	// Update is called once per frame
	void Update () {      
        MyCube.transform.position += (float)0.1 * Vector3.left;
	}
}
