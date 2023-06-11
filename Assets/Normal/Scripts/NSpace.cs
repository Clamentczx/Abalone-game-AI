

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSpace : MonoBehaviour
{
    public NGamePiece piece;

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


    public Transform blackPrefab;
    public Transform whitePrefab;

 
    private Vector location;

    private bool locationSet = false;


    public void Clear()
    {
        if (piece != null) Destroy(piece.gameObject);
    }


    public void SetPiece(char color)
    {

        if (piece != null && piece.color == color) { return; }


        if (piece != null) {Destroy(piece.gameObject); }

        var prefab = (color == 'B') ? blackPrefab : whitePrefab;

        piece = Instantiate(prefab, transform.position, Quaternion.identity, transform).GetComponent<NGamePiece>();
    }
}
