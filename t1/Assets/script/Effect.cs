using UnityEngine;
using System.Collections;

public class Effect
{
    GameObject g;
    public float t = 1f;
    public Effect(Vector3 p)
    {
        g = GameObject.Instantiate(Resources.Load("effect")) as GameObject;
        g.transform.position = p;
        g.gameObject.name = "effect";
    }
    public void update()
    {
        if (t < 0)
            return;
        t -= Time.deltaTime;
        if (t < 0)
            remove();
    }
    public void remove()
    {
        GameObject.Destroy(g);
    }

}








