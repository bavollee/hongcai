using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
   public Role role;
   public Role lastHitRole;
    public float m_speed = 1;
    public bool touch = false;
    public const float initForce = 0.3f;
    public float force = initForce;
    public bool isForward = true;
    bool run = false;
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
    public void reset()
    {
        lastHitRole = null;
        r.angularVelocity = Vector3.zero;
        r.velocity = Vector3.zero;
        r.Sleep();
        transform.position = role.bornPos;
        gameObject.SetActive(true);
    }
    void OnCollisionEnter(Collision c)
    {
        Move m = c.gameObject.GetComponent<Move>();
        if (m == null)
            return;
        
        //e1.SetActive(false);
        //e1.SetActive(true);
       // touch = true;
        Vector3 dir = c.gameObject.transform.position -transform.position;
        m.r.AddForce(getForce(force * 0.2f).magnitude * dir.normalized, ForceMode.Impulse);
        lastHitRole = m.role;
        //r.AddForce(getForce(force,false));
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(key))
        {
            
        }
        if (role.ai.AiOn == false)
        {
            if (Input.GetKey(key))
            {
                rush();
            }
            if (Input.GetKeyUp(key))
            {
                up();
            }
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

    public void rush()
    {
        run = true;
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
        if (role.ai.AiOn)
            return;
        if (state)
        {
            rush();
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
            r.AddForce(getForce(force, isForward));
        }
    }

    public void stopRush()
    {
        touch = false;
        run = false;
    }
    public void up()
    {
        stopRush();
        if (sp != round)
        {
            round = -round;
            sp = round;
        }
    }
    public Vector3 getForce(float f, bool dir = true)
    {
        return transform.TransformDirection((dir ? 1 : -1) * Vector3.forward * f * r.mass * r.drag * Physics.gravity.magnitude);
    }
}








