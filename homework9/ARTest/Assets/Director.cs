using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{

    public bool playing = true;
    private static Director _instance;

    public static Director GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Director();
        }
        return _instance;
    }
}