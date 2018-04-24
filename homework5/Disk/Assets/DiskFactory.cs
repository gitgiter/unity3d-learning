using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class DiskFactory : MonoBehaviour
{
    private static DiskFactory _instance;
    public FirstControl sceneControler { get; set; }
    GameObject diskPrefab;
    public DiskControl diskData;
    public List<GameObject> used;
    public List<GameObject> free;
    // Use this for initialization

    public static DiskFactory getInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = Singleton<DiskFactory>.Instance;
            _instance.used = new List<GameObject>();
            _instance.free = new List<GameObject>();
            diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk"), new Vector3(40, 0, 0), Quaternion.identity);
        }
        Debug.Log("instance: " + _instance);
    }
    public void Start()
    {
        sceneControler = (FirstControl)Director.getInstance().sceneCtrl;
        Debug.Log(sceneControler);
        //Debug.Log(this);
        //Debug.Log(_instance);
        sceneControler.factory = _instance;
        Debug.Log("DiskFactory: factory");
        //Debug.Log(sceneControler.factory);        
    }

    public GameObject getDisk(int round)
    {
        if (sceneControler.scoreRecorder.Score >= round * 10)
        {
            if (sceneControler.user.round < 3)
            {
                sceneControler.user.round++;
                sceneControler.user.num = 0;
                sceneControler.scoreRecorder.Score = 0;
            }
            else
            {
                sceneControler.user.game = 2;
            }
        }
        else
        {
            if (sceneControler.user.num >= 10)
            {
                sceneControler.user.game = 1;
            }            
        }
        GameObject newDisk;
        RoundControl diskOfCurrentRound = new RoundControl(round);        
        if (free.Count == 0) // if no free disk, then create a new disk
        {
            newDisk = GameObject.Instantiate(diskPrefab) as GameObject;
            newDisk.AddComponent<ClickGUI>();
            diskData = newDisk.AddComponent<DiskControl>();
        }
        else // else let the first free disk be the newDisk
        {
            newDisk = free[0];
            free.Remove(free[0]);
            newDisk.SetActive(true);
            Debug.Log("get from free");
        }
        diskData = newDisk.GetComponent<DiskControl>();
        diskData.color = diskOfCurrentRound.color;
        //Debug.Log("free: " + free.Count);

        newDisk.transform.localScale = diskOfCurrentRound.scale * diskPrefab.transform.localScale;
        newDisk.GetComponent<Renderer>().material.color = diskData.color;
        
        used.Add(newDisk);
        return newDisk;
    }

    public void freeDisk(GameObject disk1)
    {
        used.Remove(disk1);
        disk1.SetActive(false);
        free.Add(disk1);
        Debug.Log("free: " + free.Count);
        return;
    }

    public void Restart()
    {
        used.Clear();
        free.Clear();
    }
}
