using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    GUIStyle LabelStyle1;
    GUIStyle LabelStyle2;
    GUIStyle ButtonStyle;
    public int score = 0;
    public static float time = 0;

    public int round = 1;
    public int CoolTimes = 3;
    public int game = 0; // status

    // Use this for initialization
    void Start()
    {
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

    void Update()
    {
        //check key input and decide whether the hero should play animation
        GameObject hero = ((FirstControl)Director.getInstance().sceneCtrl).gameModel.getHero();        
        bool keyPressed = false;        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            hero.GetComponent<Animation>().Play("Run");
            keyPressed = true;
            action.heroMove(Diretion.UP);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            hero.GetComponent<Animation>().Play("Run");
            keyPressed = true;
            action.heroMove(Diretion.DOWN);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            hero.GetComponent<Animation>().Play("Run");
            keyPressed = true;
            action.heroMove(Diretion.LEFT);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            hero.GetComponent<Animation>().Play("Run");
            keyPressed = true;
            action.heroMove(Diretion.RIGHT);
        }
        if (Input.GetKey(KeyCode.J))
        {
            keyPressed = true;
            hero.GetComponent<Animation>().Play("Attack");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            keyPressed = true;
            hero.GetComponent<Animation>().Play("Jump", PlayMode.StopAll);
        }
        if (keyPressed == false)
        {                    
            hero.GetComponent<Animation>().Play("idle");
        }

        //cool time system
        time += Time.deltaTime;
        if (time < 1)
            return;
        time = 0;

        if (game == 3)
        {
            if (CoolTimes > 1) CoolTimes--;
            else game = 0;
        }        
    }

    public void Restart()
    {
        CoolTimes = 3;
        game = 3;
        score = 0;
    }

    public void Begin()
    {
        Restart();
    }

    void OnGUI()
    {
        if (game == 0) // playing
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 160, 100, 50), "Score: " + score, LabelStyle1);
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
        else if (game == 2) // win
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

    void OnEnable()
    {
        GameEventManager.myGameScoreAction += getScore;
        GameEventManager.myGameOverAction += gameOver;
    }

    void OnDisable()
    {
        GameEventManager.myGameScoreAction -= getScore;
        GameEventManager.myGameOverAction -= gameOver;
    }

    void getScore()
    {
        score++;
    }

    void gameOver()
    {
        game = 1;
    }
}
