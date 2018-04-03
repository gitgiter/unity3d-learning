using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class ClickGUI : MonoBehaviour {
    public ItemControl itemCtrl;
    public UserAction action;

	// Use this for initialization
	void Start () {
        action = (UserAction)Diretor.getInstance().sceneCtrl;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (gameObject.name == "Boat") action.BoatMove();
        else action.ItemClick(itemCtrl);
    }
}
