using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Camera.main != null)
        {
            Vector3 v = Camera.main.transform.position - transform.position;
            v.y = 0.0f;
            transform.forward = v;
        }
	
	}
}
