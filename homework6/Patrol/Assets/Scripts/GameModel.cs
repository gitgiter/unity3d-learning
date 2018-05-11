using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class GameModel : SSActionManager, ISSActionCallback {    
    private GameObject myHero;
    private List<GameObject> PatrolSet;
    private List<int> PatrolLastDir;

    private const float normalSpeed = 0.5f;
    private const float catchingSpeed = 0.8f;

    protected void Start () {
        FirstControl scene = (FirstControl)Director.getInstance().sceneCtrl;
        scene.gameModel = this;
        Debug.Log("in GameModel start");
        loadHero();
        loadPatrols();
    }

    protected new void Update() {
        base.Update();
    }

    //load hero from prefab
    void loadHero() {
        Debug.Log("load hero");
        //myHero = Instantiate(Resources.Load<GameObject>("Prefabs/hero"), new Vector3(348, 15, 324), Quaternion.identity);
        myHero = GameObject.Find("hero");
    }

    public GameObject getHero() {
        return myHero;
    }

    //get patrol from factory
    void loadPatrols() {
        PatrolSet = new List<GameObject>(6);
        PatrolLastDir = new List<int>(6);
        Vector3[] posSet = PatrolFactory.getInstance().getPosSet();
        for (int i = 0; i < 6; i++) {
            GameObject newPatrol = PatrolFactory.getInstance().getPatrol();
            newPatrol.transform.position = posSet[i];
            newPatrol.name = "Patrol" + i;
            PatrolLastDir.Add(-2);
            PatrolSet.Add(newPatrol);
            addRandomMovement(newPatrol, true);
        }
    }

    //hero移动
    public void heroMove(int dir) {
        myHero.transform.rotation = Quaternion.Euler(new Vector3(0, dir * 90, 0));
        switch (dir) {
            case Diretion.UP:
                myHero.transform.position += new Vector3(0, 0, 0.1f);
                break;
            case Diretion.DOWN:
                myHero.transform.position += new Vector3(0, 0, -0.1f);
                break;
            case Diretion.LEFT:
                myHero.transform.position += new Vector3(-0.1f, 0, 0);
                break;
            case Diretion.RIGHT:
                myHero.transform.position += new Vector3(0.1f, 0, 0);
                break;
        }
    }

    //动作结束后
    public void SSActionEvent(SSAction source, bool catchState) {
        if (catchState)
        {
            addDirectMovement(source.GameObject);
        }
        else
        {
            addRandomMovement(source.GameObject, true);
        }            
    }

    //isActive说明是否主动变向（动作结束）
    public void addRandomMovement(GameObject sourceObj, bool isActive) {
        int index = getIndex(sourceObj);
        int randomDir = getRandomDirection(index, isActive);
        PatrolLastDir[index] = randomDir;

        sourceObj.transform.rotation = Quaternion.Euler(new Vector3(0, randomDir * 90, 0));
        Vector3 target = sourceObj.transform.position;
        Debug.Log(target);
        Debug.Log(randomDir);
        switch (randomDir) {
            case Diretion.UP:
                target += new Vector3(0, 0, 5);
                break;
            case Diretion.DOWN:
                target += new Vector3(0, 0, -5);
                break;
            case Diretion.LEFT:
                target += new Vector3(-5, 0, 0);
                break;
            case Diretion.RIGHT:
                target += new Vector3(5, 0, 0);
                break;
        }
        Debug.Log(target);
        addSingleMoving(sourceObj, target, normalSpeed, false);
    }

    int getIndex(GameObject sourceObj) {
        string name = sourceObj.name;
        return name[name.Length - 1] - '0';
    }

    int getRandomDirection(int index, bool isActive) {
        int randomDir = Random.Range(-1, 3);
        if (!isActive) {    //当碰撞时，不走同方向
            while (PatrolLastDir[index] == randomDir) {
                randomDir = Random.Range(-1, 3);
            }
        }
        else {              //当非碰撞时，不走反方向
            while (PatrolLastDir[index] == 0 && randomDir == 2 
                || PatrolLastDir[index] == 2 && randomDir == 0
                || PatrolLastDir[index] == 1 && randomDir == -1
                || PatrolLastDir[index] == -1 && randomDir == 1) {
                randomDir = Random.Range(-1, 3);
            }
        }
        //Debug.Log(isActive + " isActive " + "PatrolLastDir " + PatrolLastDir[index] + " -- randomDir " + randomDir);
        return randomDir;
    }

    //追捕hero
    public void addDirectMovement(GameObject sourceObj) {
        int index = getIndex(sourceObj);
        PatrolLastDir[index] = -2;

        sourceObj.transform.LookAt(sourceObj.transform);
        Vector3 oriTarget = myHero.transform.position - sourceObj.transform.position;
        Vector3 target = new Vector3(oriTarget.x / 4.0f, 0, oriTarget.z / 4.0f);
        target += sourceObj.transform.position;
        //Debug.Log("addDirectMovement: " + target);
        addSingleMoving(sourceObj, target, catchingSpeed, true);
    }

    void addSingleMoving(GameObject sourceObj, Vector3 target, float speed, bool isCatching) {
        this.AddAction(sourceObj, SSMoveToAction.GetSSMoveToAction(target, speed, isCatching), this);
    }

    void addCombinedMoving(GameObject sourceObj, Vector3[] target, float[] speed, bool isCatching) {
        List<SSAction> acList = new List<SSAction>();
        for (int i = 0; i < target.Length; i++) {
            acList.Add(SSMoveToAction.GetSSMoveToAction(target[i], speed[i], isCatching));
        }
        SequenceAction MoveSeq = SequenceAction.GetSequenceAction(acList);
        this.AddAction(sourceObj, MoveSeq, this);
    }

    //获取hero所在区域
    public Transform getHeroPosition() {
        return myHero.GetComponent<HeroControl>().heroPosition;
    }
}
