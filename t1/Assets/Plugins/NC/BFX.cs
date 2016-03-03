using UnityEngine;
using System.Collections;

public class BFX : MonoBehaviour {

	int uvAnimationTileX = 4; //Here you can place the number of columns of your sheet. 
                           //The above sheet has 24

	int uvAnimationTileY = 1; //Here you can place the number of rows of your sheet. 
	                          //The above sheet has 1
	float framesPerSecond = 15f;
	
	void Update () {
	
	        // Calculate index
	        int index = (int)(Time.time * framesPerSecond);
	        // repeat when exhausting all frames
	        index = (int)index % (uvAnimationTileX * uvAnimationTileY);
	
	        // Size of every tile
	        Vector2 size =new Vector2 (1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);
	
	        // split into horizontal and vertical index
	        var uIndex = index % uvAnimationTileX;
	        var vIndex = index / uvAnimationTileX;
	
	        // build offset
	        // v coordinate is the bottom of the image in opengl so we need to invert.
	        Vector2 offset = new Vector2 (uIndex * size.x, 1.0f - size.y - vIndex * size.y);
	
	        renderer.material.SetTextureOffset ("_MainTex", offset);
	        renderer.material.SetTextureScale ("_MainTex", size);
	}
}
