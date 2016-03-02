using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : MonoBehaviour {

    GameObject g;
    Move m;
    List<KeyCode> key = new List<KeyCode>() {KeyCode.Q,KeyCode.P, KeyCode.M , KeyCode.Z};
    List<Vector3> v3 = new List<Vector3>();
    List<Role> roleList = new List<Role>();
	// Use this for initialization
    int max = 4;
	void Awake ()
    {
        //重力
        Physics.gravity = Vector3.down * 10f;


        for (int i = 1; i < max+1; i++)
        {
            //m = GameObject.Find("b" + i).GetComponent<Move>();
            Role r = addRole(i);
            r.move.setBtn(GameObject.Find("Top" + i));
            r.move.setKey(key[i - 1]);
            v3.Add(r.move.transform.position);
        }
        UIEventListener.Get(GameObject.Find("Top5")).onClick = reset;
	}
    Role addRole(int id)
    {
        float ra = 5f;
        float v = 2*Mathf.PI / max;
        v = v * id + Mathf.PI / 4;
        Vector3 v3 = new Vector3(ra * Mathf.Sin(v), 0, ra * Mathf.Cos(v));
        Role  role= new Role(id,v3);
        roleList.Add(role);
       return role;
    }
    private void reset(GameObject go)
    {
        for (int i = 0; i < roleList.Count; i++)
        {
            roleList[i].move.r.Sleep();
            //mList[i].r.sleepAngularVelocity = 5;
            //mList[i].r.sleepVelocity = 5;
            roleList[i].move.transform.position = v3[i];
        }
    }

	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.Escape))
    {
        Application.Quit();
    }
	}
}
