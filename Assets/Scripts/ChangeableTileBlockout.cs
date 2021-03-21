using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableTileBlockout : MonoBehaviour
{
    public TileType StartType;
    public TileType EndType;

    [HideInInspector]
    public Vector2Int MinTile; // Inclusive
    [HideInInspector]
    public Vector2Int MaxTile; // Inclusive

    private bool HasSwapped = false;

    void Awake()
    {
        if (StartType == TileType.TT_Undefined || EndType == TileType.TT_Undefined)
        {
            Debug.LogError("Found ChangeableTileBlockout with type TT_Undefined");
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

    public void SwapTiles()
	{
        if (HasSwapped)
		{
            return;
		}
        FindObjectOfType<TowerLevel>().ReplaceTiles(EndType, MinTile.x, MinTile.y, MaxTile.x, MaxTile.y);
        HasSwapped = true;
	}
}
