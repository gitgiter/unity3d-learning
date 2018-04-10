using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour {

    private static int bwh = 60; // button's width and height
    private static int ml = 350; // margin left
    private static int mt = 100; // margin top

    private bool turn;
    private int count;
    private int[, ] state = new int[3, 3];
    private int[, ] sequence = new int[3, 3];
    private GUIStyle LabelStyle = new GUIStyle ();

    // Use this for initialization
    void Start () {
        Restart ();

        LabelStyle.fontSize = 18;
        LabelStyle.fontStyle = FontStyle.BoldAndItalic;
        LabelStyle.normal.textColor = Color.yellow;
    }

    // Update is called once per frame
    void Update () {

    }

    void Restart () {
        turn = false;
        count = 0;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++) {
                state[i, j] = -1;
                sequence[i, j] = 0;
            }
    }

    void Regret () {
        Debug.Log ("enter");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (count > 0 && sequence[i, j] == count) {
                    Debug.Log (sequence[i, j]);
                    sequence[i, j] = 0;
                    state[i, j] = -1;
                    turn = turn ? false : true;
                    count--;
                    return;
                }
            }
        }
        Debug.Log ("quit");
    }

    int Check () {
        for (int i = 0; i < 3; i++) {
            if (state[i, 0] != -1)
                if (state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
                    return state[i, 0]; // Three rows
            if (state[0, i] != -1)
                if (state[0, i] == state[1, i] && state[1, i] == state[2, i])
                    return state[0, i]; // Three columns
        }

        if (state[1, 1] != -1) {
            if (state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2]) return state[1, 1];
            if (state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0]) return state[1, 1];
        }

        if (count == 9) return 3;
        return -1;
    }

    string LabelText (int status) {
        if (status == -1) {
            if (turn) return "Player2(x) is playing";
            else return "Player1(o) is playing";
        } else if (status == 0) return "Player1(o) wins!";
        else if (status == 1) return "Player2(x) wins!";
        else return "Nobody win";
    }

    string ButtonText (int status) {
        if (status == -1) return "";
        else if (status == 0) return "o";
        else return "x";
    }

    private void OnGUI () {
        if (GUI.Button (new Rect (ml + bwh / 2 - 10, mt - 50, bwh, 30), "Restart")) Restart ();
        if (GUI.Button (new Rect (ml + bwh * 3 / 2 + 10, mt - 50, bwh, 30), "Regret")) Regret ();
        GUI.Label (new Rect (ml, mt + 180, 3 * bwh, 30), LabelText (Check ()), LabelStyle);

        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (GUI.Button (new Rect (i * bwh + ml, j * bwh + mt, bwh, bwh), ButtonText (state[i, j]))) {
                    if (state[i, j] == -1 && Check () == -1) {
                        state[i, j] = turn ? 1 : 0;
                        turn = turn ? false : true;
                        sequence[i, j] = ++count;
                        Debug.Log (Check ());
                    }
                }
            }
        }
    }
}