using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    GUIStyle LabelStyle1;
    GUIStyle LabelStyle2;
    GUIStyle ButtonStyle;
    Score score;

    // Use this for initialization
    void Awake()
    {
        score = GetComponent<Score>();
        Debug.Log(score);        

        LabelStyle1 = new GUIStyle();
        LabelStyle1.fontSize = 20;
        LabelStyle1.alignment = TextAnchor.MiddleCenter;

        LabelStyle2 = new GUIStyle();
        LabelStyle2.fontSize = 30;
        LabelStyle2.alignment = TextAnchor.MiddleCenter;

        ButtonStyle = new GUIStyle("Button");
        ButtonStyle.fontSize = 20;
    }

    void Update()
    {

    }

    public void Continue()
    {
        score.SetGameStatus(0);
    }

    void OnGUI()
    {
        if (score != null)
        {
            //playing
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 280, 100, 50), "Host: " + score.GetHostScore(), LabelStyle1);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 250, 100, 50), "Client: " + score.GetClientScore(), LabelStyle1);

            if (score.GetGameStatus() == 1) // host win
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Host win!", LabelStyle2);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Continue", ButtonStyle))
                {
                    Continue();
                }
            }
            else if (score.GetGameStatus() == 2) // client win
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Client win!", LabelStyle2);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Continue", ButtonStyle))
                {
                    Continue();
                }
            }
        }        
    }
}