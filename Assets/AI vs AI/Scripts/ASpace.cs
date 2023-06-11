// Space is a MonoBehaviour that represents a space on the displayed board.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASpace : MonoBehaviour
{
    // Reference to the piece located at this space, if any. Null if space is empty.
    public AGamePiece piece;

    // The location of this space on the board.
    public Vector Location
    {
        get
        {
            return location;
        }
        set
        {
            if (!locationSet) { location = value; locationSet = true; }
            else throw new System.AccessViolationException("Location of a space cannot be changed.");
        }
    }

    // black and white pieces prefabs
    public Transform blackPrefab;
    public Transform whitePrefab;

    // Private backing field for Location property.
    private Vector location;
    // Boolean flag indicating whether or not location has already been set.
    private bool locationSet = false;



    // destroy piece object in this space
    public void Clear()
    {
        if (piece != null) Destroy(piece.gameObject);
    }



    // create pice in this space
    public void SetPiece(char color)
    {
   
        if (piece != null && piece.color == color){  
		return;
	}


       
        if (piece != null) {
		Destroy(piece.gameObject); 
	}
       
        var prefab = (color == 'B') ? blackPrefab : whitePrefab;
        piece = Instantiate(prefab, transform.position, Quaternion.identity, transform).GetComponent<AGamePiece>();
    }
}
