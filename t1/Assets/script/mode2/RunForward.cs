using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RunForward : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool isForward = true;

    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Run()
    {
        Vector3 dir = isForward ? Vector3.forward : Vector3.back;
        _rb.AddForce(transform.TransformDirection(dir) * _rb.mass * _rb.drag * moveSpeed);
    }
}
