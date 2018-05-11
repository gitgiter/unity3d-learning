using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

//public class ActionManager : SSActionManager
//{
//    public FirstControl sceneController;
//    public PatrolFactory patrolFactory;
//    public GameObject patrol;
//    // Use this for initialization
//    protected void Start()
//    {
//        sceneController = (FirstControl)Director.getInstance().sceneCtrl;
//        patrolFactory = sceneController.factory;
//        sceneController.MyActionManager = this;
//    }

//    // Update is called once per frame
//    protected new void Update()
//    {
//        base.Update();
//    }

//    //public void playDisk(int round)
//    //{
//    //    this.AddAction(Disk, EmitDisk, this);
//    //}

//    public void SSActionEvent(SSAction source)
//    {

//    }
//}

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
    public bool isCatching;//whether the patrol is catching

    private SSMoveToAction() { }

    public static SSMoveToAction GetSSMoveToAction(Vector3 target, float speed, bool catchState)
    {
        SSMoveToAction action = CreateInstance<SSMoveToAction>();
        action.des = target;
        action.speed = speed;
        action.isCatching = catchState;
        return action;
    }

    public override void Start() { }

    public override void Update()
    {
        Transform.position = Vector3.MoveTowards(Transform.position, des, speed * Time.deltaTime);
        if (Transform.position == des)
        {
            destroy = true;
            //according to whether is catching, different action response
            Callback.ActionDone(this, isCatching);
        }
    }
}
public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;
    //different
    public static SequenceAction GetSequenceAction(List<SSAction> sequence, int repeat = 0, int start = 0)
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

    public void ActionDone(SSAction source, bool catchState = false)
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
        //new
        for (int i = 0; i < waitingAdd.Count; i++)
        {
            if (waitingAdd[i].GameObject.Equals(gameObject))
            {
                SSAction ac = waitingAdd[i];
                waitingAdd.RemoveAt(i);
                i--;
                DestroyObject(ac);
            }
        }
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.GameObject.Equals(gameObject))
            {
                ac.destroy = true;
            }
        }

        action.GameObject = gameObject;
        action.Transform = gameObject.transform;
        action.Callback = callback;
        waitingAdd.Add(action);
        action.Start();
    }

    public void ActionDone(SSAction source, bool catchState = false) { }
}