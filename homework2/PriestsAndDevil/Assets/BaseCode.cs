using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Diretor : System.Object
    {
        private static Diretor _instance;
        public SceneControl sceneCtrl { get; set; }

        public static Diretor getInstance()
        {
            if (_instance == null) return _instance = new Diretor();
            else return _instance;
        }
    }

    public interface SceneControl
    {
        void LoadPrefabs();
    }

    public interface UserAction
    {
        void BoatMove();
        void Restart();
        void ItemClick(ItemControl itemCtrl);
    }

    public class ItemControl
    {
        public GameObject item { get; set; }
        public int itemType { get; set; }
        public ClickGUI clickGUI;
        public Moveable moveable;
        public bool isOnBoat;
        public ShoreControl shoreCtrl;

        public ItemControl(string type)
        {
            if (type == "Priest")
            {
                item = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                itemType = 0;
            }
            else
            {
                item = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                itemType = 1;
            }
            moveable = item.AddComponent(typeof(Moveable)) as Moveable;

            clickGUI = item.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickGUI.itemCtrl = this;
        }
        public void MoveTo(Vector3 des)
        {
            moveable.MoveTo(des);
        }
        public void GetOnBoat(BoatControl boatCtrl)
        {
            shoreCtrl = null;
            item.transform.parent = boatCtrl.boat.transform;
            isOnBoat = true;
        }
        public void GetOnShore(ShoreControl side)
        {
            shoreCtrl = side;
            item.transform.parent = null;
            isOnBoat = false;
        }
        public void Reset()
        {
            moveable.Reset();
            ShoreControl fromShore = ((FirstContol)Diretor.getInstance().sceneCtrl).fromShore;
            GetOnShore(fromShore);
            item.transform.position = fromShore.GetEmptyPosition();
            fromShore.GetOnShore(this);
        }
    }

    public class Moveable : MonoBehaviour
    {
        public float time = 3;
        public float g = -10;
        private Vector3 v0;
        private Vector3 Gravity;
        private float dTime = 0;
        private float fixUpdateStep = 0.1f;

        private static float speed = 20;
        public int status;
        Vector3 des;
        
        void Update()
        {
            if (status == 1)
            {
                Gravity.y = g * (dTime += fixUpdateStep);
                transform.Translate(v0 * fixUpdateStep);
                transform.Translate(Gravity * fixUpdateStep);
                if (dTime >= time - fixUpdateStep)
                {
                    transform.position = des;
                    status = 0;
                    dTime = 0;
                }
            }
            else if (status == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, des, speed * fixUpdateStep);
                if (transform.position == des) status = 0;
            }
            
        }
        public void MoveTo(Vector3 target)
        {
            des = target;
            status = 1;
            if (des.y == transform.position.y) status = 2;
            //Debug.Log("From:" + transform.position);
            v0 = new Vector3((des.x - transform.position.x) / time,
                (des.y - transform.position.y) / time - 0.5f * g * time, (des.z - transform.position.z) / time);
            Gravity = Vector3.zero;
            //Debug.Log("v0:" + v0);
            //Debug.Log("Target:" + target);                       
        }
        public void Reset()
        {
            status = 0;
            dTime = 0;
        }
    }

    public class ShoreControl
    {
        public GameObject Shore;
        public Vector3 from = new Vector3(18, 2, 0);
        public Vector3 to = new Vector3(-18, 2, 0);
        public Vector3[] positions;
        public int status;
        ItemControl[] itemCtrls;

        public ShoreControl(string type)
        {
            positions = new Vector3[] {new Vector3(13, 5, 0), new Vector3(15, 5, 0), new Vector3(17, 5, 0),
                new Vector3(19, 5, 0), new Vector3(21, 5, 0), new Vector3(23, 5, 0)};

            itemCtrls = new ItemControl[6];

            if (type == "From")
            {
                Shore = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Shore", typeof(GameObject)), from, Quaternion.identity, null);
                Shore.name = "From";
                status = 1;
            }
            else
            {
                Shore = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Shore", typeof(GameObject)), to, Quaternion.identity, null);
                Shore.name = "To";
                status = -1;
            }
        }
        public int GetEmptyIndex()
        {
            for (int i = 0; i < itemCtrls.Length; i++)
                if (itemCtrls[i] == null) return i;
            return -1;
        }
        public Vector3 GetEmptyPosition()
        {
            Vector3 pos = positions[GetEmptyIndex()];
            pos.x *= status;
            return pos;
        }
        public void GetOnShore(ItemControl item)
        {
            int index = GetEmptyIndex();
            itemCtrls[index] = item;
        }
        public ItemControl GetOffShore(string name)
        {
            for (int i = 0; i < itemCtrls.Length; i++)
            {
                if (itemCtrls[i] != null && itemCtrls[i].item.name == name)
                {
                    ItemControl temp = itemCtrls[i];
                    itemCtrls[i] = null;
                    return temp;
                }
            }
            return null;
        }
        public int GetItemNum(int type)
        {
            int count = 0;
            for (int i = 0; i < itemCtrls.Length; i++)
                if (itemCtrls[i] != null && itemCtrls[i].itemType == type)
                    count++;
            return count;
        }
        public void Reset()
        {
            itemCtrls = new ItemControl[6];
        }
    }

    public class BoatControl
    {
        public GameObject boat;
        public Moveable moveable;
        public Vector3 from = new Vector3(10, 2, 0);
        public Vector3 to = new Vector3(-10, 2, 0);
        public Vector3[] froms;
        public Vector3[] tos;
        public int status; // from = 1, to = 0
        public ItemControl leftSeat;
        public ItemControl rightSeat;

        public BoatControl()
        {
            status = 1;
            leftSeat = null;
            rightSeat = null;

            froms = new Vector3[] { new Vector3(9, 3, 0), new Vector3(11, 3, 0) };
            tos = new Vector3[] { new Vector3(-11, 3, 0), new Vector3(-9, 3, 0) };

            boat = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), from, Quaternion.identity, null);           
            boat.name = "Boat";

            moveable = boat.AddComponent(typeof(Moveable)) as Moveable;
            boat.AddComponent(typeof(ClickGUI));
        }
        public void Move()
        {
            if (status == -1) moveable.MoveTo(from);
            else moveable.MoveTo(to);
            status = -status;
        }
        public bool IsEmpty()
        {
            if (leftSeat == null && rightSeat == null) return true;
            else return false;
        }
        public bool IsFull()
        {
            if (leftSeat != null && rightSeat != null) return true;
            else return false;
        }
        public Vector3 GetOnBoat(ItemControl item)
        {
            if (leftSeat == null)
            {
                leftSeat = item;
                return status == -1 ? tos[0] : froms[0];
            }
            else if (rightSeat == null)
            {
                rightSeat = item;
                return status == -1 ? tos[1] : froms[1];
            }
            else return Vector3.zero;
        }
        public ItemControl GetOffBoat(ItemControl item)
        {
            ItemControl temp = null;
            if (leftSeat == item)
            {
                temp = leftSeat;
                leftSeat = null;
            }
            else if (rightSeat == item)
            {
                temp = rightSeat;
                rightSeat = null;
            }
            return temp;
        }
        public int GetItemNum(int type)
        {
            int count = 0;
            if (leftSeat != null && leftSeat.itemType == type) count++;
            if (rightSeat != null && rightSeat.itemType == type) count++;
            return count;
        }
        public void Reset()
        {
            moveable.Reset();
            if (status == -1) Move();
            leftSeat = null;
            rightSeat = null;
        }
    }
}