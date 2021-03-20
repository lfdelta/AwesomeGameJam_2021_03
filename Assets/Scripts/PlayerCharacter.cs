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

    private int MoveDistPerTurn;
    private Vector2Int TurnStartPos;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Texture2D tex;
        switch (CharType)
        {
            case PlayerCharacterType.CT_Rogue:
                MoveDistPerTurn = 3;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Kobold/kobold_5.png", typeof(Texture2D));
                break;
            case PlayerCharacterType.CT_Fighter:
                MoveDistPerTurn = 2;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Goblin/goblin_1.png", typeof(Texture2D));
                break;
            case PlayerCharacterType.CT_Mage:
                MoveDistPerTurn = 2;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Ghost/ghost_3.png", typeof(Texture2D));
                break;
            default:
                MoveDistPerTurn = 0;
                tex = null;
                break;
        }
        renderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), tex.width);

        Level = FindObjectOfType<TowerLevel>();
        Pos = Level.PlayerSpawns[CharType];
        TurnStartPos = Pos;
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

    public void IncrementTurn()
	{
        TurnStartPos = Pos;
	}

    public Vector2Int GetPosition()
	{
        return Pos;
	}

    void Update()
    {
        if (!IsControlled)
        {
            return;
        }

        if (Input.GetButtonDown("MoveLeft"))
		{
            if (CanMoveTo(Pos.x - 1, Pos.y))
            {
                --Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveRight"))
		{
            if (CanMoveTo(Pos.x + 1, Pos.y))
            {
                ++Pos.x;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveUp"))
		{
            if (CanMoveTo(Pos.x, Pos.y + 1))
            {
                ++Pos.y;
                UpdateWorldPosition();
            }
        }
        if (Input.GetButtonDown("MoveDown"))
		{
            if (CanMoveTo(Pos.x, Pos.y - 1))
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

    private bool CanMoveTo(int X, int Y)
	{
        if (!Level.IsTileTraversable(CharType, X, Y))
		{
            return false;
		}
        return Mathf.Abs(TurnStartPos.x - X) + Mathf.Abs(TurnStartPos.y - Y) <= MoveDistPerTurn;
    }
}
