using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : MonoBehaviour {

    GameObject g;
    Move m;
    List<KeyCode> key = new List<KeyCode>() {KeyCode.Q,KeyCode.P, KeyCode.M , KeyCode.Z};
    List<Role> roleList = new List<Role>();
    List<GameObject> btnList = new List<GameObject>();
    Dictionary<int, bool> player = new Dictionary<int, bool>();
    int max = 4;
    GameObject startBtn;
    GameObject endBtn;
    GameObject resetBtn;
    PropMgr _propMgr;
	void Awake ()
    {
        //重力
        Physics.gravity = Vector3.down * 30f;
        for (int i = 0; i < max ; i++)
        {
            btnList.Add(GameObject.Find("Top" + i));
            UIEventListener.Get(btnList[i]).onClick = chooseRole;
            player.Add(i, false);
        }
        resetBtn = GameObject.Find("Top5");
        startBtn = GameObject.Find("Top6");
        endBtn = GameObject.Find("Top7");
        endBtn.SetActive(false);
        _propMgr = gameObject.GetComponent<PropMgr>();
        _propMgr.enabled = false;
        UIEventListener.Get(startBtn).onClick = startGame;
	}

    private void chooseRole(GameObject go)
    {
        int id = btnList.IndexOf(go);
        player[id] = !player[id];
    }
    void startGame(GameObject g)
    {
        startBtn.SetActive(false);
        for (int i = 0; i < max; i++)
        {
            //m = GameObject.Find("b" + i).GetComponent<Move>();
            Role r = addRole(i + 1, player[i]);
            r.setControl(btnList[i], key[i]);
        }
        _propMgr.enabled = true;
        UIEventListener.Get(resetBtn).onClick = reset;
    }

    Role addRole(int id,bool player = false)
    {
        float ra = 5f;
        float v = 2*Mathf.PI / max;
        v = v * id + Mathf.PI / 4+Mathf.PI;
        Vector3 v3 = new Vector3(ra * Mathf.Sin(v), 0, ra * Mathf.Cos(v));
        Role  role= new Role(id,v3,player);
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

        foreach (var item in roleList)
        {
            if (item.score < maxScore)
                item.die(true);
            else
                item.win();
        }
        endBtn.SetActive(true);
        resetBtn.SetActive(false);

    }
	// Update is called once per frame
	void Update () {
        foreach (var item in roleList)
        {
            item.update();
        }
        foreach (var item in roleList)
        {
            maxScore = Mathf.Max(maxScore, item.score);
        }
        if(maxScore >=5)
        {
            endGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
