using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTileMap : MonoBehaviour
{
    [SerializeField]
    private TilemapCollider2D collider;

    [SerializeField]
    private TilemapRenderer renderer;

    public void Activate()
    {
        Break();
    }

    public void Break()
    {
        collider.enabled = false;
        renderer.enabled = false;   
    }


}
