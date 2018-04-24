using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PhysicsEmit : SSAction
{
    bool enableEmit = true;
    Vector3 force;
    float startX;
    float targetZ = 50;
    public FirstControl sceneControler = (FirstControl)Director.getInstance().sceneCtrl;
    // Use this for initialization
    public override void Start()
    {
        startX = 6 - Random.value * 12;
        this.Transform.position = new Vector3(startX, 0, 0);
        force = new Vector3(6 * Random.Range(-1, 1), 6 * Random.Range(0.5f, 2), 13 + 2 * sceneControler.user.round);
    }
    public static PhysicsEmit GetSSAction()
    {
        PhysicsEmit action = ScriptableObject.CreateInstance<PhysicsEmit>();
        return action;
    }
    // Update is called once per frame
    public override void Update()
    {
        if (!this.destroy)
        {
            if (enableEmit)
            {
                Debug.Log("add force");
                GameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                enableEmit = false;
            }
        }
        if (this.Transform.position.z >= targetZ)
        {
            MyDestroy();
        }
    }
    public void MyDestroy()
    {
        //Debug.Log(GameObject);
        //Debug.Log("Destroy");
        //DiskFactory.getInstance().freeDisk(gameobject);
        GameObject.SetActive(false);
        GameObject.transform.position = new Vector3(startX, 0, 0);
        sceneControler.factory.freeDisk(GameObject);
        this.destroy = true;
        this.Callback.ActionDone(this);
    }
    
    public override void FixedUpdate()
    {
        //Debug.Log("in fixedupdate");
        //if (!this.destroy)
        //{
        //    if (enableEmit)
        //    {
        //        Debug.Log("add force");
        //        GameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //        GameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        //        enableEmit = false;
        //    }
        //}
    }
}