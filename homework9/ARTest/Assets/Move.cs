using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour, IVirtualButtonEventHandler
{
    private Vector3 up;
    private Vector3 middle;
    private Vector3 down;

    public GameObject dragon;
    public GameObject up_btn;
    public GameObject down_btn;
    public VirtualButtonBehaviour[] vbs;

    // Use this for initialization  
    void Start()
    {
        vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
        dragon = GameObject.Find("SJ001");
        up_btn = GameObject.Find("Up");
        down_btn = GameObject.Find("Down");

        up = new Vector3(0, 1f * 0.03f, 0);
        middle = new Vector3(0, 0.5f * 0.03f, 0);
        down = new Vector3(0, 0, 0);
    }

    // Update is called once per frame  
    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " btn pressed");
        
        switch (vb.VirtualButtonName)
        {
            case "Up":
                GameObject.Find("Up_Sphere").transform.localScale *= 1.3f;
                Debug.Log("from " + dragon.transform.position);               
                if (Mathf.Abs(dragon.transform.position.y - down.y) < 0.001)
                {
                    dragon.transform.position = middle;
                    dragon.GetComponent<BoxCollider>().center = middle / 0.03f;
                }                    
                else
                {
                    dragon.transform.position = up;
                    dragon.GetComponent<BoxCollider>().center = up / 0.03f;
                }                    
                Debug.Log("up to " + dragon.transform.position);

                break;
            case "Down":
                GameObject.Find("Down_Sphere").transform.localScale *= 1.3f;
                Debug.Log("from " + dragon.transform.position);
                if (Mathf.Abs(dragon.transform.position.y - up.y) < 0.001)
                {
                    dragon.transform.position = middle;
                    dragon.GetComponent<BoxCollider>().center = middle / 0.03f;
                }                    
                else
                {
                    dragon.transform.position = down;
                    dragon.GetComponent<BoxCollider>().center = down / 0.03f;
                }                    
                Debug.Log("down to " + dragon.transform.position);
                
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        GameObject.Find("Up_Sphere").transform.localScale = new Vector3(1, 0.1f, 1);
        GameObject.Find("Down_Sphere").transform.localScale = new Vector3(1, 0.1f, 1);
    }
}