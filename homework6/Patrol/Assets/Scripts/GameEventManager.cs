using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class GameEventManager : MonoBehaviour {
    public delegate void GameScoreAction();
    public static event GameScoreAction myGameScoreAction;

    public delegate void GameOverAction();
    public static event GameOverAction myGameOverAction;

    private FirstControl scene;

    void Start () {
        scene = (FirstControl)Director.getInstance().sceneCtrl;
        scene.gameEventManager = this;
    }
	
	void Update () {
		
	}

    //hero escape, get score
    public void heroEscapeAndScore() {
        if (myGameScoreAction != null)
            myGameScoreAction();
    }

    //hero gets caught, game over
    public void patrolHitHeroAndGameover() {
        if (myGameOverAction != null)
            myGameOverAction();
    }
}
