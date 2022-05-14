using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTileMap : MonoBehaviour
{
    [SerializeField]
    private TilemapCollider2D myCollider;

    [SerializeField]
    private TilemapRenderer myRenderer;

    public void Activate()
    {
        Break();
    }

    public void Break()
    {
        myCollider.enabled = false;
        myRenderer.enabled = false;   
    }


}
