using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Role  {

    GameObject g;
    public Move move;
    public int id;
    public Role(int id,Vector3 pos)
    {
        this.id = id;
        g =GameObject.Instantiate(Resources.Load("ball")) as GameObject;
        g.name = "ball" + id;
        Material m = Resources.Load("ball_1"+id) as Material;
        g.renderer.material = m;
        g.transform.localScale = 3 * Vector3.one;
        g.transform.position = pos;
        g.transform.LookAt(Vector3.zero);
        move = g.AddComponent<Move>();
    }
}
