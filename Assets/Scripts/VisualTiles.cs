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
        TilePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObjects/TileSprite.prefab", typeof(GameObject));

        Assets = new Dictionary<TileType, Texture2D[]>();
        Assets[TileType.TT_Open] = new Texture2D[]
            {
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/tile_1.png", typeof(Texture2D)),
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/tile_2.png", typeof(Texture2D)),
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/tile_3.png", typeof(Texture2D)),
            };
        Assets[TileType.TT_Wall] = new Texture2D[] { (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/wall_1.png", typeof(Texture2D)) };
        Assets[TileType.TT_MagePassable] = new Texture2D[] { (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/mage_wall_1.png", typeof(Texture2D)) };
        Assets[TileType.TT_Breakable] = new Texture2D[] { (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/fallen_rock_1.png", typeof(Texture2D)) };
        Assets[TileType.TT_Door] = new Texture2D[] { (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/door_front_view_1.png", typeof(Texture2D)) };
        Assets[TileType.TT_LevelEnd] = new Texture2D[] { (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/exit_1.png", typeof(Texture2D)) };

        WalkableTileAsset = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/walkable_tile_overlay.png", typeof(Texture2D));
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
