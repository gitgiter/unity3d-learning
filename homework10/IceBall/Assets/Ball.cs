using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	
    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        var speed = GetComponent<Rigidbody>().velocity;
        //Debug.Log("speed: " + speed);        

        //如果没有碰到上下边界，操作待扩展
        if (transform.position.z > -6 && transform.position.z < 6)
        {
            //transform.Translate(new Vector3(0, 0, speed.z));            
        }
        //碰到上下边界
        else
        {
            //碰到门，更改比分，重置球位置
            if (transform.position.x > -1.5 && transform.position.x < 1.5)
            {
                var score = GetComponent<Score>();
                Debug.Log(score);
                
                if (transform.position.z > 0)
                    score.HostWin();
                else score.ClientWin();

                transform.position = Vector3.zero;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            //z轴速度反向
            else
            {
                speed.z = -speed.z;
                GetComponent<Rigidbody>().velocity = speed;
                Debug.Log("up down bound speed: " + speed);
            }            
        }
        
        //如果没有碰到左右边界，操作待扩展
        if (transform.position.x > -3.5 && transform.position.x < 3.5)
        {
            //transform.Translate(new Vector3(speed.x, 0, 0));
        }
        //碰到左右边界，x轴速度反向
        else
        {
            speed.x = -speed.x;
            GetComponent<Rigidbody>().velocity = speed;
            Debug.Log("left right bound speed:" + speed);
        }

        //速度衰减
        //speed *= 0.999f;
        //GetComponent<Rigidbody>().velocity = speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponent<PlayerMove>();

        if (hitPlayer != null)
        {
            var ballRigid = GetComponent<Rigidbody>();
            Debug.Log("velocity: " + ballRigid.velocity);
            //适当加速
            //ballRigid.velocity *= 2;
        }
    }
}
