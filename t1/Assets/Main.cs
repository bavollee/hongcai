using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : MonoBehaviour {

    GameObject g;
    Move m;
    List<KeyCode> key = new List<KeyCode>() {KeyCode.Q,KeyCode.P, KeyCode.M , KeyCode.Z};
    List<Move> mList = new List<Move>();
    List<Vector3> v3 = new List<Vector3>();
	// Use this for initialization
	void Awake ()
    {
        //重力
        Physics.gravity = Vector3.down * 10f;

        Resources.Load("ball");

        for (int i = 1; i < 3; i++)
        {
            m = GameObject.Find("b" + i).GetComponent<Move>();
            m.setBtn(GameObject.Find("Top" + i));
            m.setKey(key[i - 1]);
            v3.Add(m.transform.position);
            mList.Add(m);
        }
        UIEventListener.Get(GameObject.Find("Top5")).onClick = reset;
	}

    private void reset(GameObject go)
    {
        for (int i = 0; i < mList.Count; i++)
        {
            mList[i].r.Sleep();
            mList[i].r.sleepAngularVelocity = 5;
            mList[i].r.sleepVelocity = 5;
            mList[i].transform.position = v3[i];
        }
    }

    private void lHd(GameObject go, bool state)
    {
        if(state)
            m.down();
        else
            m.up();
    }

	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.Escape))
    {
        Application.Quit();
    }
	}
}
