using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PhysicsActionManager : SSActionManager
{
    public FirstControl sceneController;
    public DiskFactory diskFactory;
    public RecordControl scoreRecorder;
    public PhysicsEmit EmitDisk;
    public GameObject Disk;
    int count = 0;
    // Use this for initialization
    protected void Start()
    {
        sceneController = (FirstControl)Director.getInstance().sceneCtrl;
        diskFactory = sceneController.factory;
        scoreRecorder = sceneController.scoreRecorder;
        //sceneController.MyActionManager = this;
        sceneController.myAdapter.SetPhysicsAM(this);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    public void playDisk(int round)
    {
        EmitDisk = PhysicsEmit.GetSSAction();
        Disk = diskFactory.getDisk(round);
        this.AddAction(Disk, EmitDisk, this);
        Disk.GetComponent<DiskControl>().action = EmitDisk;
    }

    public void SSActionEvent(SSAction source)
    {
        if (!source.GameObject.GetComponent<DiskControl>().hit)
            scoreRecorder.miss();
        diskFactory.freeDisk(source.GameObject);
        source.GameObject.GetComponent<DiskControl>().hit = false;
    }
}

public class ActionManager : SSActionManager
{
    public FirstControl sceneController;
    public DiskFactory diskFactory;
    public RecordControl scoreRecorder;
    public Emit EmitDisk;
    public GameObject Disk;
    // Use this for initialization
    protected void Start()
    {
        sceneController = (FirstControl)Director.getInstance().sceneCtrl;
        diskFactory = sceneController.factory;
        scoreRecorder = sceneController.scoreRecorder;
        //sceneController.MyActionManager = this;
        sceneController.myAdapter.SetNormalAM(this);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    public void playDisk(int round)
    {
        //Debug.Log(diskFactory);
        EmitDisk = Emit.GetSSAction();
        Disk = diskFactory.getDisk(round);
        this.AddAction(Disk, EmitDisk, this);
        Disk.GetComponent<DiskControl>().action = EmitDisk;
    }

    public void SSActionEvent(SSAction source)
    {
        if (!source.GameObject.GetComponent<DiskControl>().hit)
            scoreRecorder.miss();
        diskFactory.freeDisk(source.GameObject);
        source.GameObject.GetComponent<DiskControl>().hit = false;
    }
}

public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;

    public GameObject GameObject { get; set; }
    public Transform Transform { get; set; }
    public ISSActionCallback Callback { get; set; }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }

    public virtual void FixedUpdate()
    {
        //throw new System.NotImplementedException();
    }
}

public class SSMoveToAction : SSAction
{
    public Vector3 des;
    public float speed;

    private SSMoveToAction() { }

    public static SSMoveToAction GetSSMoveToAction(Vector3 target, float speed)
    {
        SSMoveToAction action = CreateInstance<SSMoveToAction>();
        action.des = target;
        action.speed = speed;
        return action;
    }

    public override void Start() { }

    public override void Update()
    {
        Transform.position = Vector3.MoveTowards(Transform.position, des, speed * Time.deltaTime);
        if (Transform.position == des)
        {
            destroy = true;
            Callback.ActionDone(this);
        }
    }
}
public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;

    public static SequenceAction GetSequenceAction(int repeat, int start, List<SSAction> sequence)
    {
        SequenceAction action = CreateInstance<SequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    public void ActionDone(SSAction source)
    {
        source.destroy = false;
        start++;
        if (start >= sequence.Count)
        {
            start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                destroy = true;
                Callback.ActionDone(this);
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.GameObject = GameObject;
            action.Transform = Transform;
            action.Callback = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        foreach (SSAction action in sequence)
        {
            DestroyObject(action);
        }
    }
}

public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction action in waitingAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction action = kv.Value;
            if (action.destroy)
            {
                waitingDelete.Add(action.GetInstanceID());
            }
            else if (action.enable)
            {
                action.Update();
                //action.FixedUpdate();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
        }
        waitingDelete.Clear();
    }

    public void AddAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
    {
        action.GameObject = gameObject;
        action.Transform = gameObject.transform;
        action.Callback = callback;
        waitingAdd.Add(action);
        action.Start();
    }

    public void ActionDone(SSAction source) { }
}