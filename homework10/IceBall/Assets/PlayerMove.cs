using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public GameObject ballPrefab;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 scanPos;

    // Use this for initialization
    void Start () {
        scanPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        //判断是哪边的玩家
        if (transform.position.z < 0)
        {
            //向前不能越过中线，向后不能超出边界
            if (transform.position.z + z < 0 && transform.position.z + z > -6)
            {                
                transform.Translate(0, 0, z);
            }      
            //向左向右都不能超出边界
            if (transform.position.x + x < 3.5 && transform.position.x + x > -3.5)
            {
                transform.Translate(x, 0, 0);
            }
        }
        else
        {
            //向前不能越过中线，向后不能超出边界
            if (transform.position.z - z > 0 && transform.position.z - z < 6)
            {
                transform.Translate(0, 0, -z);
            }
            //向左向右都不能超出边界
            if (transform.position.x - x < 3.5 && transform.position.x - x > -3.5)
            {
                transform.Translate(-x, 0, 0);
            }
        }

    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        offset = scanPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!isLocalPlayer)
            return;
        
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        var temp = transform.position;
        //判断是哪边的玩家
        if (transform.position.z < 0)
        {
            //向前不能越过中线，向后不能超出边界
            if (curPosition.z < -0.5 && curPosition.z > -6)
            {
                temp.z = curPosition.z;
                transform.position = temp;
            }
            //向左向右都不能超出边界
            if (curPosition.x < 3.5 && curPosition.x > -3.5)
            {
                temp.x = curPosition.x;
                transform.position = temp;
            }
        }
        else
        {
            //向前不能越过中线，向后不能超出边界
            if (curPosition.z > 0.5 && curPosition.z < 6)
            {
                temp.z = curPosition.z;
                transform.position = temp;
            }
            //向左向右都不能超出边界
            if (curPosition.x < 3.5 && curPosition.x > -3.5)
            {
                temp.x = -curPosition.x;
                transform.position = temp;
            }
        }
        Debug.Log("curPosition" + curPosition);
    }

    public override void OnStartLocalPlayer()
    {
        //创建冰球，且唯一
        if (GameObject.Find("Ball(Clone)") == null)
            CmdStart();

        //取消对方的摄像机
        if (transform.position.z < 0)
        {
            GameObject.Find("Camera").SetActive(false);
        }

        //设置己方颜色
        MeshRenderer[] temp = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in temp)
        {
            m.material.color = Color.red;
        }
    }    

    [Command]
    void CmdStart()
    {
        // This [Command] code is run on the server!

        // create the bullet object locally
        var ball = (GameObject)Instantiate(
             ballPrefab,
             new Vector3(0, 0.002f, 0),
             Quaternion.identity);

        // spawn the bullet on the clients
        NetworkServer.Spawn(ball);        
    }    
}
