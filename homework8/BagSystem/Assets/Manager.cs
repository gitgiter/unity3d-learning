using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    private MouseImage mouse;
    private int IsWeapon1 = 0;
    private int IsWeapon2 = 0;
    private int IsWeapon3 = 0;

    // Use this for initialization
    void Start()
    {
        mouse = (MouseImage)FindObjectOfType(typeof(MouseImage));
        for (int i = 1; i <= 9; i++)
        {
            GameObject.Find("Grid" + i).AddComponent<Bag>();
        }
        for (int i = 1; i <= 3; i++)
        {
            GameObject.Find("Weapon" + i).AddComponent<Equip>();
        }
    }

    // Update is called once per frame
    public MouseImage getMouse()
    {
        return mouse;
    }

    public void setMouse(MouseImage m)
    {
        if (mouse == null)
        {
            mouse = m;
        }
    }

    public int getWeapon1()
    {
        return IsWeapon1;
    }
    public int getWeapon2()
    {
        return IsWeapon2;
    }
    public int getWeapon3()
    {
        return IsWeapon3;
    }
    public void setWeapon1(int w1)
    {
        IsWeapon1 = w1;
    }
    public void setWeapon2(int w2)
    {
        IsWeapon2 = w2;
    }
    public void setWeapon3(int w3)
    {
        IsWeapon3 = w3;
    }
}
