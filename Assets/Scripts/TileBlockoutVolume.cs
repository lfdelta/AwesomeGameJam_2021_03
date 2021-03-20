using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class TileBlockoutVolume : MonoBehaviour
{
    public TileType Type;

    [HideInInspector]
    public Vector2Int MinTile; // Inclusive
    [HideInInspector]
    public Vector2Int MaxTile; // Inclusive

    void Awake()
    {
        if (Type == TileType.TT_Undefined)
        {
            Debug.LogError("Found TileBlockoutVolume with type TT_Undefined");
        }
        Transform transf = GetComponent<Transform>();
        MinTile.x = Mathf.FloorToInt(transf.position.x / TileUtils.TileSize);
        MinTile.y = Mathf.FloorToInt(transf.position.y / TileUtils.TileSize);
        MaxTile.x = Mathf.CeilToInt((transf.position.x + transf.localScale.x) / TileUtils.TileSize) - 1;
        MaxTile.y = Mathf.CeilToInt((transf.position.y + transf.localScale.y) / TileUtils.TileSize) - 1;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite)
		{
            sprite.enabled = false;
		}
    }
}
