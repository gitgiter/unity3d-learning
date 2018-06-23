using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Score : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [SyncVar]
    public int score1 = 0;
    [SyncVar]
    public int score2 = 0;
    [SyncVar]
    public int game = 0; // status: 0=>playing; 1=>player1 win; 2=>player2 win;    

    public void HostWin()
    {
        //保证在服务端上修改共享变量
        if (!isServer)
            return;

        score1++;
        game = 1;
    }

    public void ClientWin()
    {
        //保证在服务端上修改共享变量
        if (!isServer)
            return;

        score2++;
        game = 2;
    }

    public int GetHostScore()
    {
        return score1;
    }

    public int GetClientScore()
    {
        return score2;
    }

    public void SetGameStatus(int status)
    {
        game = status;
    }

    public int GetGameStatus()
    {
        return game;
    }
}
