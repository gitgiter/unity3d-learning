using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PatrolControl : MonoBehaviour {
    private IAddAction addAction;
    private IGameStatusOp gameStatusOp;

    public int whichPatrol;
    public bool isCatching; //whether found hero

    private float CATCH_RADIUS = 3.0f;

    // Use this for initialization
    void Start () {
        addAction = (FirstControl)Director.getInstance().sceneCtrl as IAddAction;
        gameStatusOp = (FirstControl)Director.getInstance().sceneCtrl as IGameStatusOp;

        whichPatrol = getIndex();
        isCatching = false;
    }

    // Update is called once per frame
    void Update () {
        //check
        if (Vector3.Distance(gameStatusOp.getHeroPosition().position, gameObject.transform.position) <= 20f)
        {   
            //hero go in the area
            //start catching
            if (!isCatching)
            {
                Debug.Log(this.gameObject + " is catching");
                isCatching = true;                
            }
            //moves direct to the hero
            addAction.addDirectMovement(this.gameObject);
        }
        else
        {
            if (isCatching)
            {
                Debug.Log(this.gameObject + " stops catching");
                //hero has moved out of the area
                //stop catching 
                gameStatusOp.heroEscapeAndScore();
                isCatching = false;                
            }
            //then moves randomly
            addAction.addRandomMovement(this.gameObject, false);
        }
    }

    public int getIndex()
    {
        //get the index of patrol
        string name = this.gameObject.name;
        return name[name.Length - 1] - '0';
    }

    void OnCollisionStay(Collision e)
    {
        //hit other patrol, move to other direction
        if (e.gameObject.name.Contains("Patrol"))
        {
            isCatching = false;
            addAction.addRandomMovement(this.gameObject, false);
        }

        //hit hero, game over
        if (e.gameObject.name.Contains("hero"))
        {
            gameStatusOp.patrolHitHeroAndGameover();
            Debug.Log("Game Over!");
        }
    }
}
