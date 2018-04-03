using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola3 : MonoBehaviour {

    private Rigidbody rigid;
    private Vector3 v0;

    // Use this for initialization
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        v0 = new Vector3(3, 10, 0);
        rigid.velocity = v0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
