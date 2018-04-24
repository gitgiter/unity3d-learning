using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class Emit : SSAction
{
    public FirstControl sceneControler = (FirstControl)Director.getInstance().sceneCtrl;
    public Vector3 target;     
    public float speed;     
    private float distanceToTarget;    
    float startX;
    float targetX;
    float targetY;

    public override void Start()
    {
        speed = sceneControler.user.round * 5;
        GameObject.GetComponent<DiskControl>().speed = speed;
        startX = 6 - Random.value * 12;
        if (Random.value > 0.5)
        {
            targetX = 36 - Random.value * 36;
        }
        else
        {
            targetX = -36 + Random.value * 36;
        }
        if (Random.value > 0.5)
        {
            targetY = 25 - Random.value * 25;
        }
        else
        {
            targetY = -25 + Random.value * 25;
        }
        //targetY = (Random.value * 25);
        this.Transform.position = new Vector3(startX, 0, 0);
        target = new Vector3(targetX, targetY, 30);
        //计算两者之间的距离  
        distanceToTarget = Vector3.Distance(this.Transform.position, target);
    }
    public static Emit GetSSAction()
    {
        Emit action = ScriptableObject.CreateInstance<Emit>();
        return action;
    }
    public override void Update()
    {
        Vector3 targetPos = target;

        //让始终它朝着目标  
        GameObject.transform.LookAt(targetPos);

        //计算弧线中的夹角  
        float angle = Mathf.Min(1, Vector3.Distance(GameObject.transform.position, targetPos) / distanceToTarget) * 45;
        GameObject.transform.rotation = GameObject.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
        float currentDist = Vector3.Distance(GameObject.transform.position, target);
        GameObject.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        if (this.Transform.position == target)
        {
            Debug.Log("Destroy");
            //DiskFactory.getInstance().freeDisk(gameobject);
            GameObject.SetActive(false);
            GameObject.transform.position = new Vector3(startX, 0, 0);
            sceneControler.factory.freeDisk(GameObject);
            this.destroy = true;
            this.Callback.ActionDone(this);
        }
    }
    public override void FixedUpdate()
    {
        //base.FixedUpdate();
    }
}