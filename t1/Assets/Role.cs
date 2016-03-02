using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Role  {

    GameObject g;
    public Move move;
    public Role(Vector3 p)
    {
        g = Resources.Load("ball") as GameObject;
        g.transform.position = p;
        move = g.GetComponent<Move>();
    }
}
