using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class ClickGUI : MonoBehaviour {
    public DiskControl diskCtrl;
    public IUserAction action;

	// Use this for initialization
	void Start () {
        action = (IUserAction)Director.getInstance().sceneCtrl;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        action.Hit(diskCtrl);
    }
}
