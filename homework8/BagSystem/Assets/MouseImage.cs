using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseImage : MonoBehaviour {

    private Manager GM;
    private Image mouseImage;
    private int mouseType = 0;
    private int z;

    public Sprite none;
    public Sprite weapon1;
    public Sprite weapon2;
    public Sprite weapon3;

    // Use this for initialization
    void Start()
    {
        GM = (Manager)FindObjectOfType(typeof(Manager));
        GM.setMouse(this);
        mouseImage = GetComponent<Image>();        
        z = -400;

        weapon1 = GameObject.Find("Grid1").GetComponent<Image>().sprite;
        weapon2 = GameObject.Find("Grid2").GetComponent<Image>().sprite;
        weapon3 = GameObject.Find("Grid3").GetComponent<Image>().sprite;
        none = GameObject.Find("MouseImage").GetComponent<Image>().sprite;
    }

    public int getMouseType()
    {
        return mouseType;
    }
    public void setMouseType(int m)
    {
        mouseType = m;
        switch (m)
        {
            case 0:
                mouseImage.sprite = none;
                z = -400;
                break;
            case 1:
                mouseImage.sprite = weapon1;
                z = 100;
                break;
            case 2:
                mouseImage.sprite = weapon2;
                z = 100;
                break;
            case 3:
                mouseImage.sprite = weapon3;
                z = 100;
                break;
            default:
                mouseImage.sprite = none;
                z = -400;
                break;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x - 26, Input.mousePosition.y - 26, z);
    }
}
