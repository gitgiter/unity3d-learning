using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

//----------------------------------
// 此脚本加在主人公hero上
//----------------------------------

public class HeroControl : MonoBehaviour {
    public Transform heroPosition;

	void Start () {
		
	}
	
	void Update () {
        GameObject hero = ((FirstControl)Director.getInstance().sceneCtrl).gameModel.getHero();
        heroPosition = hero.transform;
    }
}
