using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float m_speed = 1;
    public bool touch = false;
    public const float force = 20f;
    bool run = false;
    float addSp;
    float sp;
    float round = 300f;
    public Rigidbody r;
    public GameObject e1;
    void Awake()
    {
        r = GetComponent<Rigidbody>();

        foreach (var item in transform.GetComponentsInChildren<Transform>(true))
        {
            if (item.name == "effect")
            {
                e1 = item.gameObject;
                e1.SetActive(false);
            }
        }
    }
    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("++++++++++++++OnCollisionEnter");
        Move m = c.gameObject.GetComponent<Move>();
        if (m == null)
            return;

        
        e1.SetActive(false);
        e1.SetActive(true);
        touch = true;
        //Vector3 dir = c.gameObject.transform.position -transform.position;
         //c.gameObject.GetComponent<Move>().r.AddForce(getForce(force*0.5f).magnitude*r.velocity.normalized,ForceMode.Impulse);
        //r.AddForce(getForce(force,false));
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            
        }
        if (Input.GetKey(key))
        {
            run = true;
        }
        if (Input.GetKeyUp(key))
        {
            up();
        }

        if (run)
        {
            down();
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * sp, 0), Space.World);
        }
        //this.m_transform.Translate(new Vector3(moveh, 0, movev));
    }
    KeyCode key;
    public void setKey(KeyCode c)
    {
        key = c;
    }
    public void setBtn(GameObject g)
    {
        UIEventListener.Get(g).onPress = onPress;
    }
    private void onPress(GameObject go, bool state)
    {
        if (state)
        { 
            run = true;
        }
        else
        {
            up();
        }
    }
    
    public void down()
    {
        sp = 0;
        if (!touch)
        {
            r.AddForce(getForce());
        }
    }


    public void up()
    {
        touch = false;
        addSp = 0;
        run = false;
        round = -round;
        sp = round;
    }
    public Vector3 getForce(float f = force,bool dir = true)
    {
        return transform.TransformDirection((dir?1 :-1)*Vector3.forward* f * r.mass * r.drag);
    }


    public void useMy()
    {
        //if (addSp < 1)
        //    addSp += addSpVal;
        //sp = 0;
        //transform.Translate(Vector3.forward * Time.deltaTime * 10f * addSp, Space.Self);
    }
}








