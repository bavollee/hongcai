﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : MonoBehaviour {

    public float gameTime = 60f;

    public static Main main;
    GameObject g;
    Move m;
    List<KeyCode> key = new List<KeyCode>() {KeyCode.Q,KeyCode.P, KeyCode.M , KeyCode.Z};
    List<Role> roleList = new List<Role>();
    List<GameObject> btnList = new List<GameObject>();
    public static List<Color> btnColor = new List<Color>() { new Color(1, 0, 0), new Color(0.74f, 0, 1), new Color(1, 0.5f, 0), new Color(0, 0.45f, 0) };
    Dictionary<int, bool> player = new Dictionary<int, bool>();
    List<Effect> effect = new List<Effect>();
    int num = 4;
    GameObject startBtn;
    GameObject endBtn;
    GameObject resetBtn;
    UILabel tips;

    public static bool start = false;
    PropMgr _propMgr;
    
	void Awake ()
    {
        main = this;
        //重力
        Physics.gravity = Vector3.down * 30f;
        for (int i = 0; i < num ; i++)
        {
            btnList.Add(GameObject.Find("Top" + i));
            btnList[i].GetComponent<UISprite>().color = btnColor[i] - new Color(0, 0, 0,0.5f);
            UIEventListener.Get(btnList[i]).onClick = chooseRole;
            player.Add(i, false);
        }
        resetBtn = GameObject.Find("Top5");
        resetBtn.SetActive(false);
        startBtn = GameObject.Find("Top6");
        endBtn = GameObject.Find("Top7");
        tips = GameObject.Find("tips").GetComponent<UILabel>();
        tips.text = "choose player";
        endBtn.SetActive(false);
        _propMgr = gameObject.GetComponent<PropMgr>();
        _propMgr.enabled = false;
        UIEventListener.Get(startBtn).onClick = startGame;
        UIEventListener.Get(endBtn).onClick = back;
	}

    private void back(GameObject go)
    {
        Application.LoadLevel("driver");
    }

    private void chooseRole(GameObject go)
    {
        int id = btnList.IndexOf(go);
        player[id] = !player[id];
        if (player[id])
        {
            btnList[id].GetComponent<UISprite>().color = btnColor[id];
        }
        else
        {
            btnList[id].GetComponent<UISprite>().color = btnColor[id] - new Color(0, 0, 0,0.5f);
        }
        Debug.Log(btnList[id].GetComponent<UISprite>().color);
    }
    void startGame(GameObject g)
    {
        foreach (var item in btnList)
        {
             UIEventListener.Get(item).onClick -= chooseRole;
        }
        start = true;
        startBtn.SetActive(false);
        for (int i = 0; i < num; i++)
        {
            Role r = addRole(i + 1, player[i]);
            r.setControl(btnList[i], key[i]);
        }
        _propMgr.enabled = true;
        UIEventListener.Get(resetBtn).onClick = reset;
    }

    Role addRole(int id, bool player = false)
    {
        float ra = 5f;
        float v = 2 * Mathf.PI / num;
        v = v * id + Mathf.PI / 4 + Mathf.PI;
        Vector3 v3 = new Vector3(ra * Mathf.Sin(v), 0, ra * Mathf.Cos(v));
        Role role = new Role(id, v3, player);
        roleList.Add(role);
        return role;
    }
    private void reset(GameObject go)
    {
        for (int i = 0; i < roleList.Count; i++)
        {
            roleList[i].born();
        }
    }
    int maxScore=0;
    public void endGame()
    {
        start = false;
        _propMgr.enabled = false;
        List<Role> w = new List<Role>();
        string winner = "";
        foreach (var item in roleList)
        {
            if (item.score < maxScore)
                item.die(true);
            else
            {
                w.Add(item);
                winner += " " + item.name;
                item.win();
            }
        }
        tips.text = "winner:" + winner;
        endBtn.SetActive(true);
        resetBtn.SetActive(false);
    }

	void Update () {
        foreach (var item in roleList)
        {
            maxScore = Mathf.Max(maxScore, item.score);
        }
        if (gameTime <= 0 && start)
        {
            endGame();
        }
        else if (start)
        {
            gameTime -= Time.deltaTime;
            tips.text = "time: " + (int)gameTime;
        }
        foreach (var item in roleList)
        {
            item.update();
        }

//         if (maxScore >= 5)
//         {
//             endGame();
//        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
