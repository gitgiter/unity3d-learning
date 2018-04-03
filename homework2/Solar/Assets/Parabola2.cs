using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola2 : MonoBehaviour {

    private GameObject MyCapsule;
    private Vector3 vx;
    private Vector3 vz;
    private Vector3 a;
    private float t;
    // Use this for initialization
    void Start()
    {
        MyCapsule = GameObject.Find("Capsule");
        vx = (float)0.5 * Vector3.right;
        vz = Vector3.zero;
        a = (float)0.1 * Vector3.forward;
        t = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        MyCapsule.transform.position += vx;
        MyCapsule.transform.position += vz;
        vz += a * t;
    }
}
