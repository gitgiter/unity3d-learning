using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class ActionManager : SSActionManager
{   
    public void MoveBoat(BoatControl boatCtrl)
    {
        SSMoveToAction action;
        if (boatCtrl.status == -1) 
            action = SSMoveToAction.GetSSMoveToAction(boatCtrl.from, BoatControl.speed);
        else action = SSMoveToAction.GetSSMoveToAction(boatCtrl.to, BoatControl.speed);
        boatCtrl.status = -boatCtrl.status;
        AddAction(boatCtrl.boat, action, this);
    }

    public void MoveItem(ItemControl itemCtrl, Vector3 finalDes)
    {
        //Debug.Log("enter MoveItem!");
        float time = 3;
        float g = -10;
        Vector3 v0;
        float vy_ByGravity = 0;
        float stepTime = 0.1f;
        Vector3 currentDes = itemCtrl.item.transform.position;

        List<SSAction> divide = new List<SSAction>();

        // the des here is the final des
        v0 = new Vector3((finalDes.x - itemCtrl.item.transform.position.x) / time,
            (finalDes.y - itemCtrl.item.transform.position.y) / time - 0.5f * g * time, (finalDes.z - itemCtrl.item.transform.position.z) / time);
        //Debug.Log(v0);
        //Debug.Log(time / stepTime);


        // divide the curve to many parts
        for (int i = 0; i < time / stepTime - 1; i++)
        {
            //Debug.Log(divide[i]);
            //Debug.Log(currentDes);
            // change the vy
            vy_ByGravity += g * stepTime;
            // set current des
            currentDes += v0 * stepTime;
            currentDes.y += vy_ByGravity * stepTime;
            // get the current speed
            float currentSpeed = Mathf.Sqrt(v0.x * v0.x + (v0.y + vy_ByGravity) * (v0.y + vy_ByGravity));
            // add one of the movements
            SSAction temp = SSMoveToAction.GetSSMoveToAction(currentDes, currentSpeed * 10);
            divide.Add(temp);
        }
        SSAction seqAction = SequenceAction.GetSequenceAction(1, 0, divide);
        AddAction(itemCtrl.item, seqAction, this);
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