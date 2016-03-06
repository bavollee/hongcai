using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AutoRot : MonoBehaviour
{
    public float speed = 300f;
    public bool isRotCW = true;

    private Rigidbody _rb;


    void Start()
    {
        _rb = gameObject.rigidbody;
    }

    void Update()
    {
        float flag = isRotCW ? 1 : -1;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, flag * speed, 0) * Time.deltaTime);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }


    public void ReverseRot()
    {
        isRotCW = !isRotCW;
    }
}
