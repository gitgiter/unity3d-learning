using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class Follow : MonoBehaviour
{
    public Vector3 offset;
    private Transform playerBip;
    public float smoothing = 0.5f;

    // Use this for initialization
    void Start()
    {
        playerBip = GameObject.Find("hero").transform;
        offset = transform.position - playerBip.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = playerBip.position + offset;
        Vector3 targetPos = playerBip.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}