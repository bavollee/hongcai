using UnityEngine;
using System.Collections;

public class TSFixed : MonoBehaviour {

    public int uvAnimationTileX = 8; //Here you can place the number of columns of your sheet. 
                           //The above sheet has 24

    public int uvAnimationTileY = 8; //Here you can place the number of rows of your sheet. 
	                          //The above sheet has 1

    public bool isFixed;
    public int row = 0;

    public int from;
    public int to;
    public float time;

    private float _timer;
    private int _totalFrame;
	void Start () 
    {
        // split into horizontal and vertical index
        _totalFrame = to - from;
        if (isFixed)
        {
            setTex(from, row);
        }
       
	}
	// Update is called once per frame
	void Update () 
    {
        if (!isFixed)
        {
            _timer += Time.deltaTime;
            int index = from + (int)(_timer * _totalFrame/time) % (_totalFrame + 1);
            setTex(index,row);
        }
	}

    private void setTex(int uIndex, int vIndex)
    {
        // Size of every tile
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);
        int u = uIndex % uvAnimationTileX;
        int v = vIndex + (int)(uIndex / uvAnimationTileX);
        // build offset
        // v coordinate is the bottom of the image in opengl so we need to invert.
        Vector2 offset = new Vector2(u * size.x, 1.0f - size.y - v * size.y);

        renderer.material.SetTextureOffset("_MainTex", offset);
        renderer.material.SetTextureScale("_MainTex", size);
    }
}
