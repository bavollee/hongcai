using UnityEngine;
using System.Collections;

public class wrap : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
	}
}
