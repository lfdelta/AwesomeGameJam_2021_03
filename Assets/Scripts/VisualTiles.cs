using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisualTiles : MonoBehaviour
{
    private static bool Initialized = false;
    private static Dictionary<TileType, Texture2D> Assets;
    private static GameObject TilePrefab;

    public static void Initialize()
	{
        if (Initialized)
		{
            return;
		}
        TilePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObjects/TileSprite.prefab", typeof(GameObject));

        Assets = new Dictionary<TileType, Texture2D>();
        Assets[TileType.TT_Open] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/tile_1.png", typeof(Texture2D));
        Assets[TileType.TT_Wall] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/wall_1.png", typeof(Texture2D));
        Assets[TileType.TT_MagePassable] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/mage_wall_1.png", typeof(Texture2D));
        Assets[TileType.TT_Breakable] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/mage_wall_1.png", typeof(Texture2D));
        Assets[TileType.TT_Door] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/mage_wall_1.png", typeof(Texture2D));

        // TODO: add more types here; maybe use an array for each one for randomized options

        Initialized = true;
	}

    
    public static GameObject CreateTile(TileType Type, int X, int Y)
    {
        if (!Initialized)
		{
            return null;
		}
        
        GameObject tile = Instantiate(TilePrefab, new Vector3(X * TileUtils.TileSize, Y * TileUtils.TileSize, 0.0f), Quaternion.identity);
        Texture2D tex = Assets[Type];
        SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
        sprite.sprite = Sprite.Create(Assets[Type], new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f,0.0f), tex.width);
        return tile;
    }
}
