using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using UnityEngine.SceneManagement;

public class FirstControl : MonoBehaviour, ISceneControl, IGameStatusOp, IUserAction, IAddAction
{
    //public ActionManager MyActionManager { get; set; }
    public GameEventManager gameEventManager;
    public GameModel gameModel;
    public PatrolFactory factory { get; set; }
    public UserGUI user;
    public static float time = 0;

    void Awake()
    {
        Director diretor = Director.getInstance();
        diretor.sceneCtrl = this;                              
    }

    // Use this for initialization
    void Start()
    {
        Begin();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void LoadPrefabs()
    {
    }

    public void Begin()
    {
        LoadPrefabs();
        //MyActionManager = gameObject.AddComponent<ActionManager>() as ActionManager;        
        gameEventManager = gameObject.AddComponent<GameEventManager>() as GameEventManager;
        //gameModel = gameObject.AddComponent<GameModel>() as GameModel;
        Debug.Log("game model init");
        user = gameObject.AddComponent<UserGUI>();
        user.Begin();
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene");
    }

    public void heroMove(int dir)
    {
        gameModel.heroMove(dir);
    }

    public void addRandomMovement(GameObject sourceObj, bool isActive)
    {
        gameModel.addRandomMovement(sourceObj, isActive);
    }

    public void addDirectMovement(GameObject sourceObj)
    {
        gameModel.addDirectMovement(sourceObj);
    }

    public Transform getHeroPosition()
    {
        return gameModel.getHeroPosition();
    }

    public void heroEscapeAndScore()
    {
        gameEventManager.heroEscapeAndScore();
    }

    public void patrolHitHeroAndGameover()
    {
        gameEventManager.patrolHitHeroAndGameover();
    }
}
