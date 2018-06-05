using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour {

    private Manager GM;
    private Image weaponImage;
    private GameObject hero;
    public int mouseType;
    public Sprite weapon1;
    public Sprite weapon2;
    public Sprite weapon3;
    public Sprite none;

    // Use this for initialization
    void Start () {
        GM = (Manager)FindObjectOfType(typeof(Manager));
        weaponImage = GetComponent<Image>();
        hero = GameObject.Find("Hero");
        weapon1 = GameObject.Find("Grid1").GetComponent<Image>().sprite;
        weapon2 = GameObject.Find("Grid2").GetComponent<Image>().sprite;
        weapon3 = GameObject.Find("Grid3").GetComponent<Image>().sprite;
        none = GameObject.Find("Grid4").GetComponent<Image>().sprite;
        Debug.Log(weapon1);
        Debug.Log(weapon2);
        Debug.Log(weapon3);
        Debug.Log(none);
        this.GetComponent<Button>().onClick.AddListener(OnEquipButton);
        mouseType = this.name[this.name.Length - 1] - '0';
        Debug.Log(mouseType);
    }
	
	// Update is called once per frame
	void Update () {
        if (hero.GetComponent<Animation>().IsPlaying("Attack") == false)
        {
            hero.GetComponent<Animation>().Play("idle", PlayMode.StopAll);
        }
        if (mouseType == 1 && GM.getWeapon1() == 1)
        {
            GM.setWeapon1(0);
            weaponImage.sprite = weapon1;
        }
        else if (mouseType == 2 && GM.getWeapon2() == 1)
        {
            GM.setWeapon2(0);
            weaponImage.sprite = weapon2;
        }
        else if (mouseType == 3 && GM.getWeapon3() == 1)
        {
            GM.setWeapon3(0);
            weaponImage.sprite = weapon3;
        }
    }

    public void OnEquipButton()
    {
        //Debug.Log("equiping");
        int _mouseType = GM.getMouse().getMouseType();
        Debug.Log(_mouseType);
        if (weaponImage.sprite != null && _mouseType == 0)
        {
            weaponImage.sprite = none;
            GM.getMouse().setMouseType(mouseType);
        }
        else if (weaponImage.sprite == none && _mouseType == mouseType)
        {
            switch (_mouseType)
            {
                case 0:
                    weaponImage.sprite = none;
                    break;
                case 1:
                    GM.setWeapon1(1);
                    weaponImage.sprite = weapon1;
                    break;
                case 2:
                    GM.setWeapon2(1);
                    weaponImage.sprite = weapon2;
                    break;
                case 3:
                    GM.setWeapon3(1);
                    weaponImage.sprite = weapon3;
                    break;
                default:
                    weaponImage.sprite = none;
                    break;
            }
            GM.getMouse().setMouseType(0);
            Animation animation = hero.GetComponent<Animation>();
            animation.Play("Attack", PlayMode.StopAll);
            animation.wrapMode = WrapMode.Once;
        }        
    }
}
