using UnityEngine;
using System.Collections;

public class Ai : MonoBehaviour
{
    public bool AiOn = false;
    public Move move;
    void Awake()
    {
        move = GetComponent<Move>();
    }
    float t = 0f;
    void Update()
    {
        if(AiOn)
        {
            t += Time.deltaTime;
            if (t > 0.2f)
            {
                t = 0;

                if (Vector3.Angle(transform.forward, -new Vector3(transform.position.x, 0, transform.position.z)) < 90 || Vector3.Distance(transform.position, Vector3.one) < .2f)
                {
                    move.rush();
                }
                else
                {
                    move.up();
                }
            }
        }
    }
    public static float changeAngle(float v)
    {
        return v * 180 / Mathf.PI;
    }
}








