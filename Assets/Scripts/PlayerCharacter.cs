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
    private bool IsControlled = false;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // TODO: sprites for different players
        Texture2D tex;
        switch (CharType)
        {
            case PlayerCharacterType.CT_Rogue:
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Kobold/kobold_5.png", typeof(Texture2D));
                break;
            case PlayerCharacterType.CT_Fighter:
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/EmptySquare.png", typeof(Texture2D));
                renderer.color = Color.blue;
                break;
            case PlayerCharacterType.CT_Mage:
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Environment/EmptySquare.png", typeof(Texture2D));
                renderer.color = Color.green;
                break;
            default:
                tex = null;
                break;
        }
        renderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), tex.width);

        Level = FindObjectOfType<TowerLevel>();
        Pos = Level.PlayerSpawns[CharType];
        UpdateWorldPosition();
    }

    public void RemoveControl()
	{
        IsControlled = false;
	}

    public void AcquireControl()
	{
        IsControlled = true;
	}

    void Update()
    {
        if (!IsControlled)
		{
            return;
		}

        // TODO: use input system for remappable keys
        if (Input.GetButtonDown("MoveLeft"))
		{
            if (Level.IsTileTraversable(CharType, Pos.x - 1, Pos.y))
            {
                --Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveRight"))
		{
            if (Level.IsTileTraversable(CharType, Pos.x + 1, Pos.y))
            {
                ++Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveUp"))
		{
            if (Level.IsTileTraversable(CharType, Pos.x, Pos.y + 1))
            {
                ++Pos.y;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveDown"))
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
        gameObject.transform.position = new Vector3(Pos.x * TileUtils.TileSize, Pos.y * TileUtils.TileSize, 0.0f);
    }
}
