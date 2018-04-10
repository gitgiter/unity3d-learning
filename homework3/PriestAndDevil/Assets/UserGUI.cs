using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class UserGUI : MonoBehaviour {
    private UserAction action;
    public int status = 0;
    GUIStyle LabelStyle1;
    GUIStyle LabelStyle2;
    GUIStyle ButtonStyle;

    public int step;
    public int timeLeft;
    // Use this for initialization
    void Start () {
        action = (UserAction)Diretor.getInstance().sceneCtrl;

        LabelStyle1 = new GUIStyle();
        LabelStyle1.fontSize = 20;
        LabelStyle1.alignment = TextAnchor.MiddleCenter;

        LabelStyle2 = new GUIStyle();
        LabelStyle2.fontSize = 30;
        LabelStyle2.alignment = TextAnchor.MiddleCenter;

        ButtonStyle = new GUIStyle("Button");
        ButtonStyle.fontSize = 20;
    }

    void FixedUpdate()
    {
        Time.fixedDeltaTime = 1;
        timeLeft--;
    }

    public void Restart()
    {
        step = 0;
        timeLeft = 60;
    }

    void OnGUI () {
        if (status == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), "Time: " + timeLeft, LabelStyle1);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 80, 100, 50), "Step: " + step, LabelStyle1);
            if (timeLeft == 0) status = 1;
        }
        else if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle))
            {
                status = 0;
                action.Restart();
            }
        }
        else if (status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle))
            {
                status = 0;
                action.Restart();
            }
        }
    }

}
