using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCharacter : MonoBehaviour
{
    public PlayerCharacterType CharType;

    private Vector2Int Pos;
    private TowerLevel Level;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // TODO: sprites for different players
        Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/EmptySquare.png", typeof(Texture2D));
        renderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), (float)tex.width / 0.8f);

        Level = FindObjectOfType<TowerLevel>();
        Pos = Level.PlayerSpawns[CharType];
        UpdateWorldPosition();
    }

    void Update()
    {
        // TODO: use input system for remappable keys
        if (Input.GetKeyDown(KeyCode.A))
		{
            if (Level.IsTileTraversable(CharType, Pos.x - 1, Pos.y))
            {
                --Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
		{
            if (Level.IsTileTraversable(CharType, Pos.x + 1, Pos.y))
            {
                ++Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
		{
            if (Level.IsTileTraversable(CharType, Pos.x, Pos.y + 1))
            {
                ++Pos.y;
                UpdateWorldPosition();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
		{
            if (Level.IsTileTraversable(CharType, Pos.x, Pos.y - 1))
            {
                --Pos.y;
                UpdateWorldPosition();
            }
        }
    }

    private void UpdateWorldPosition()
	{
        Debug.Log("Position: " + Pos.x + ", " + Pos.y);
        gameObject.transform.position = new Vector3(Pos.x * TileUtils.TileSize, Pos.y * TileUtils.TileSize, 0.0f);
    }
}
