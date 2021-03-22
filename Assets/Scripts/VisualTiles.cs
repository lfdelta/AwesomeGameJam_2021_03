using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisualTiles : MonoBehaviour
{
    private static bool Initialized = false;
    private static Dictionary<TileType, Texture2D[]> Assets;
    private static Texture2D WalkableTileAsset;
    private static GameObject TilePrefab;

    public static void Initialize()
	{
        if (Initialized)
		{
            return;
		}        
        GameAssets assets = FindObjectOfType<GameAssets>();
        TilePrefab = assets.TilePrefab;
        Assets = new Dictionary<TileType, Texture2D[]>();
        Assets[TileType.TT_Open] = assets.FloorTiles;
        Assets[TileType.TT_Wall] = assets.WallTiles;
        Assets[TileType.TT_MagePassable] = assets.MagePassableTiles;
        Assets[TileType.TT_Breakable] = assets.BreakableTiles;
        Assets[TileType.TT_Door] = assets.DoorTiles;
        Assets[TileType.TT_LevelEnd] = assets.LevelEndTiles;

        WalkableTileAsset = assets.WalkableTileOverlay;
        Initialized = true;
	}

    public static GameObject CreateTile(TileType Type, int X, int Y)
    {
        if (!Initialized)
        {
            return null;
        }
        GameObject tile = Instantiate(TilePrefab, new Vector3(X * TileUtils.TileSize, Y * TileUtils.TileSize, 0.0f), Quaternion.identity);
        Texture2D[] texPool = Assets[Type];
        Texture2D tex = texPool[Random.Range(0, texPool.Length)];
        SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
        sprite.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), tex.width);
        return tile;
    }

    public static GameObject CreateTileOverlay()
	{
        if (!Initialized)
        {
            return null;
        }
        GameObject tile = Instantiate(TilePrefab, Vector3.zero, Quaternion.identity);
        Texture2D tex = WalkableTileAsset;
        SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
        sprite.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), tex.width);
        return tile;
    }
}
