using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour {

    private Manager GM;
    private Image gridImage;
    private int mouseType = 0;

    public Sprite none;
    public Sprite weapon1;
    public Sprite weapon2;
    public Sprite weapon3;

    // Use this for initialization
    void Start () {
        GM = (Manager)FindObjectOfType(typeof(Manager));
        gridImage = GetComponent<Image>();
        weapon1 = GameObject.Find("Grid1").GetComponent<Image>().sprite;
        weapon2 = GameObject.Find("Grid2").GetComponent<Image>().sprite;
        weapon3 = GameObject.Find("Grid3").GetComponent<Image>().sprite;
        none = GameObject.Find("Grid4").GetComponent<Image>().sprite;
        Debug.Log(gridImage.sprite == weapon1);
        Debug.Log(weapon1);
        Debug.Log(weapon2);
        Debug.Log(weapon3);
        this.GetComponent<Button>().onClick.AddListener(OnBagButton);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBagButton()
    {
        //Debug.Log("grid click");
        mouseType = GM.getMouse().getMouseType();
        if (gridImage.sprite == none && mouseType != 0)
        {
            switch (mouseType)
            {
                case 1:
                    gridImage.sprite = weapon1;
                    break;
                case 2:
                    gridImage.sprite = weapon2;
                    break;
                case 3:
                    gridImage.sprite = weapon3;
                    break;
                default:
                    gridImage.sprite = none;
                    break;
            }
            GM.getMouse().setMouseType(0);          
        }
        else
        {
            //Debug.Log(gridImage.sprite);
            //Debug.Log(mouseType);
            if (gridImage.sprite == none)
            {
                GM.getMouse().setMouseType(0);
            }
            else if (gridImage.sprite == weapon1)
            {
                GM.getMouse().setMouseType(1);
            }
            else if (gridImage.sprite == weapon2)
            {
                GM.getMouse().setMouseType(2);
            }
            else if (gridImage.sprite == weapon3)
            {
                GM.getMouse().setMouseType(3);
            }

            switch (mouseType)
            {
                case 0:                    
                    gridImage.sprite = none;
                    break;
                case 1:
                    gridImage.sprite = weapon1;
                    break;
                case 2:
                    gridImage.sprite = weapon2;
                    break;
                case 3:
                    gridImage.sprite = weapon3;
                    break;
                default:
                    gridImage.sprite = none;
                    break;
            }                 
        }
    }
}
