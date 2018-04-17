using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using UnityEngine.SceneManagement;

public class FirstControl : MonoBehaviour, ISceneControl, IUserAction {

    public ActionManager MyActionManager { get; set; }
    public DiskFactory factory { get; set; }
    public RecordControl scoreRecorder;
    public UserGUI user;    
    
    void Awake()
    {
        Director diretor = Director.getInstance();
        diretor.sceneCtrl = this;
        Debug.Log("FirstControl: factory");
        Debug.Log(factory);
        Debug.Log(diretor);                                
    }

    // Use this for initialization
    void Start()
    {
        Begin();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadPrefabs()
    {

    }

    public void Begin()
    {
        LoadPrefabs();
        MyActionManager = gameObject.AddComponent<ActionManager>() as ActionManager;
        Debug.Log(MyActionManager);
        scoreRecorder = gameObject.AddComponent<RecordControl>();
        user = gameObject.AddComponent<UserGUI>();
        user.Begin();
    }

    public void Hit(DiskControl diskCtrl)
    {        
        if (user.game == 0)
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.GetType());
                //Debug.Log(hit.transform);
                hit.collider.gameObject.SetActive(false);
                Debug.Log("Hit");
                hit.collider.gameObject.GetComponent<DiskControl>().hit = true;
                scoreRecorder.add();
            }
            else
            {
                Debug.Log("Miss");
                scoreRecorder.miss();
            }
        }
        //user.status = Check();
    }
    public void PlayDisk()
    {
        MyActionManager.playDisk(user.round);
    }
    public void Restart()
    {
        SceneManager.LoadScene("scene");
    }
    public int Check()
    {
        return 0;
    }
}
