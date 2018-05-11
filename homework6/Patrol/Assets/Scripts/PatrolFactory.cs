using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PatrolFactory : MonoBehaviour
{
    private static PatrolFactory _instance;
    public FirstControl sceneControler { get; set; }
    private GameObject patrolPrefab;    
    public List<GameObject> used;
    public List<GameObject> free;
    // Use this for initialization

    private Vector3[] PatrolPosSet = new Vector3[] { new Vector3(358, 16, 318), new Vector3(349, 16, 319),
            new Vector3(372, 16, 318), new Vector3(371, 16, 297), new Vector3(383, 16, 287), new Vector3(380, 15, 251)};

    public static PatrolFactory getInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = Singleton<PatrolFactory>.Instance;
            _instance.used = new List<GameObject>();
            _instance.free = new List<GameObject>();
            patrolPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/patrol"), new Vector3(348, 15, 324), Quaternion.identity);
        }
        Debug.Log("instance: " + _instance);
    }

    public void Start()
    {
        sceneControler = (FirstControl)Director.getInstance().sceneCtrl;
        sceneControler.factory = _instance;    
    }

    public GameObject getPatrol()
    {
        GameObject temp = Instantiate(patrolPrefab);
        Debug.Log(temp);
        return temp;
    }

    public Vector3[] getPosSet()
    {
        return PatrolPosSet;
    }

    public void freePatrol(GameObject disk1)
    {
        
    }

    public void Restart()
    {
        used.Clear();
        free.Clear();
    }
}
