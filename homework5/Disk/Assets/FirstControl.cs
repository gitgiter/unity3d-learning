using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using UnityEngine.SceneManagement;

public class FirstControl : MonoBehaviour, ISceneControl, IUserAction {

    //public ActionManager MyActionManager { get; set; }
    public ActionManagerAdapter myAdapter;
    public DiskFactory factory { get; set; }
    public RecordControl scoreRecorder;
    public UserGUI user;
    public static float time = 0;
    
    void Awake()
    {
        Director diretor = Director.getInstance();
        diretor.sceneCtrl = this;
        //Debug.Log("FirstControl: factory");
        //Debug.Log(factory);
        //Debug.Log(diretor);                                
    }

    // Use this for initialization
    void Start()
    {
        Begin();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void FixedUpdate()
    {
        //Debug.Log("first fixedupdate mode: " + myAdapter.mode);

        //Time.fixedDeltaTime = 1;
        time += Time.deltaTime;
        if (time < 1)
            return;
        time = 0;

        // if round <= 3 and is playing, 

        if (user.round <= 3 && user.game == 0)
        {
            PlayDisk();
            user.num++;
        }
    }

    public void LoadPrefabs()
    {
    }

    public void Begin()
    {
        LoadPrefabs();
        //MyActionManager = gameObject.AddComponent<ActionManager>() as ActionManager;
        //Debug.Log(MyActionManager);
        myAdapter = new ActionManagerAdapter(gameObject);
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
                //hit.collider.gameObject.transform.position = new Vector3(6 - Random.value * 12, 0, 0);
                //factory.freeDisk(hit.collider.gameObject);
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
        //MyActionManager.playDisk(user.round);
        myAdapter.PlayDisk(user.round);
    }
    public void Restart()
    {
        SceneManager.LoadScene("scene");
    }
    public void SwitchMode()
    {
        Debug.Log("Switch Mode");
        myAdapter.SwitchActionMode();
    }
    public int Check()
    {
        return 0;
    }
}
