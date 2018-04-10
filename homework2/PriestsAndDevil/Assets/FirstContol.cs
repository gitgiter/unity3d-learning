using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class FirstContol : MonoBehaviour, SceneControl, UserAction {

    private ItemControl[] itemCtrls;
    public ShoreControl fromShore;
    public ShoreControl toShore;
    public BoatControl boat;
    UserGUI user;
    
    // Use this for initialization
	void Start () {
        Diretor diretor = Diretor.getInstance();
        diretor.sceneCtrl = this;
        itemCtrls = new ItemControl[6];
        LoadPrefabs();
        user = gameObject.AddComponent<UserGUI>();
        user.Restart();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadPrefabs()
    {
        GameObject water = (GameObject)Instantiate(Resources.Load("Prefabs/Water", typeof(GameObject)), Vector3.up, Quaternion.identity, null);
        water.name = "Water";
        fromShore = new ShoreControl("From");
        toShore = new ShoreControl("To");
        boat = new BoatControl();

        for (int i = 0; i < 3; i++)
        {
            ItemControl item = new ItemControl("Priest");
            item.item.name = "Priest" + i;
            item.item.transform.position = fromShore.GetEmptyPosition();
            item.GetOnShore(fromShore);
            fromShore.GetOnShore(item);

            itemCtrls[i] = item;
        }

        for (int i = 0; i < 3; i++)
        {
            ItemControl item = new ItemControl("Devil");
            item.item.name = "Devil" + i;
            item.item.transform.position = fromShore.GetEmptyPosition();
            item.GetOnShore(fromShore);
            fromShore.GetOnShore(item);

            itemCtrls[i + 3] = item;
        }
    }
    public void BoatMove()
    {
        Debug.Log("BoatMove");
        if (boat.IsEmpty()) return;
        boat.Move();
        user.step++;
        user.status = Check();
    }
    public void ItemClick(ItemControl itemCtrl)
    {
        Debug.Log("ItemClick");
        if (itemCtrl.isOnBoat)
        {
            ShoreControl side;
            if (boat.status == 1) side = fromShore;
            else side = toShore;

            Debug.Log(side.GetEmptyPosition());
            boat.GetOffBoat(itemCtrl);
            itemCtrl.MoveTo(side.GetEmptyPosition());
            itemCtrl.GetOnShore(side);
            side.GetOnShore(itemCtrl);
            user.step++;
        }
        else
        {
            if (boat.IsFull()) return;

            Debug.Log(itemCtrl.item.name + " getting on boat");
            ShoreControl side = itemCtrl.shoreCtrl;
            if (side.status != boat.status) return;

            side.GetOffShore(itemCtrl.item.name);//
            itemCtrl.MoveTo(boat.GetOnBoat(itemCtrl));
            itemCtrl.GetOnBoat(boat);
            user.step++;
        }
        user.status = Check();
    }
    public void Restart()
    {
        user.Restart();
        fromShore.Reset();
        toShore.Reset();
        boat.Reset();
        for (int i = 0; i < itemCtrls.Length; i++)
            itemCtrls[i].Reset();
    }
    public int Check()
    {
        int from_priest = fromShore.GetItemNum(0);
        int from_devil = fromShore.GetItemNum(1);
        int to_priest = toShore.GetItemNum(0);
        int to_devil = toShore.GetItemNum(1);

        if (boat.status == 1)
        {
            from_priest += boat.GetItemNum(0);
            from_devil += boat.GetItemNum(1);
        }
        else
        {
            to_priest += boat.GetItemNum(0);
            to_devil += boat.GetItemNum(1);
        }

        if (from_priest > 0 && from_priest < from_devil) return 1;
        if (to_priest > 0 && to_priest < to_devil) return 1;
        Debug.Log("priest on to shore " + toShore.GetItemNum(0));
        Debug.Log("devil on to shore " + toShore.GetItemNum(1));
        if (toShore.GetItemNum(0) + toShore.GetItemNum(1) == 6) return 2;
        return 0;
    }
}
