using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Role  {
    GameObject g;
    public Move move;
    public Ai ai;
    public int id;
    public Vector3 bornPos;
    public int score = 0;
    public UILabel txt;
    public bool dead;
    public Role(int id,Vector3 pos,bool player = false)
    {
        this.id = id;
        this.bornPos = pos;
        g =GameObject.Instantiate(Resources.Load("ball")) as GameObject;
        move = g.GetComponent<Move>();
        move.role = this;
        ai = g.GetComponent<Ai>();
        ai.AiOn = !player;
        g.name = "ball" + id;

        Material m = Resources.Load("ball_1"+id) as Material;
        g.renderer.material = m;
        g.transform.localScale = 3 * Vector3.one;
        g.transform.position = pos;
        g.transform.LookAt(Vector3.zero);

    }
    public void setControl(GameObject btn,KeyCode k)
    {
        foreach (var item in btn.GetComponentsInChildren<Transform>(true))
        {
            if(item.name == "Label")
                txt = item.GetComponent<UILabel>();
        }
        txt.text = "" + score;
        move.setBtn(btn);
        move.setKey(k);
    }

    public void addScore(int s)
    {
        score += s;
        txt.text = ""+score;
    }
    const float reLiveTime = 1f;
    float tempTime = 0f;
    public void update()
    {
        if (g.transform.position.y < -2)//死亡
        {
            tempTime += Time.deltaTime;
            die();
        }
        if (dead && tempTime > reLiveTime)
        {
            tempTime = 0;
            born();
        }
    }

    public void die()
    {
            if (g.activeSelf)
            {
                dead = true;
                move.lastHitRole.addScore(1);
                g.SetActive(false);
            }
    }
    public void born()
    {
            move.reset();
            dead = false;
    }
}
