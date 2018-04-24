using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;

public class UserGUI : MonoBehaviour {
    private IUserAction action;     
    GUIStyle LabelStyle1;
    GUIStyle LabelStyle2;
    GUIStyle ButtonStyle;
    private int timeLeft = 60;
    public int score = 0;
    public static float time = 0;

    public int round = 1;
    public int CoolTimes = 3; 
    public int game = 0; // status
    public int num = 0; // numbers of disk
    public int mode = 0; // action mode

    // Use this for initialization
    void Start () {
        action = (IUserAction)Director.getInstance().sceneCtrl;

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
        time += Time.deltaTime;
        if (time < 1)
            return;
        time = 0;

        if (game == 3)
        {
            if (CoolTimes > 1) CoolTimes--;
            else game = 0;
        }
        if (game == 0)
        {
            timeLeft--;
        }
    }

    public void Restart()
    {
        timeLeft = 60;
        round = 1;
        CoolTimes = 3;
        game = 3;
        num = 0;
        score = 0;
        mode = 0;
    }

    public void Begin()
    {
        Restart();
    }

    void OnGUI () {
        string str = mode == 0 ? "Normal" : "Physics";
        GUI.Label(new Rect(Screen.width / 2 - 30, 10, 100, 50), "Mode: " + str, LabelStyle1);
        if (GUI.Button(new Rect(20, 20, 100, 50), "Switch", ButtonStyle)) // switch mode
        {            
            action.SwitchMode();
            mode = 1 - mode;
        }
        if (game == 0) // playing
        {
            GUI.Label(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 160, 100, 50), "Round: " + round, LabelStyle1);
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 160, 100, 50), "Time: " + timeLeft, LabelStyle1);
            GUI.Label(new Rect(Screen.width / 2 + 120, Screen.height / 2 - 160, 100, 50), "Score: " + score, LabelStyle1);
            if (timeLeft == 0) game = 1;
        }
        else if (game == 1) // game over
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle))
            {
                game = 0;
                action.Restart();
            }
        }
        else if (game == 2) // win a round
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle))
            {
                game = 0;
                action.Restart();
            }
        }
        else if (game == 3) // ready
        {
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.height / 2, 100, 50), CoolTimes.ToString(), LabelStyle2);
        }
    }

}
