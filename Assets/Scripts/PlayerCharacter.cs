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

    private bool HasUsedAction = false;
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
        HasUsedAction = false;
	}

    public Vector2Int GetPosition()
	{
        return Pos;
	}

    void Update()
    {
        if (!IsControlled || HasUsedAction)
        {
            return;
        }

        if (Input.GetButtonDown("MoveLeft"))
		{
            TryMoveToward(Pos.x - 1, Pos.y);
        }
        if (Input.GetButtonDown("MoveRight"))
		{
            TryMoveToward(Pos.x + 1, Pos.y);
        }
        if (Input.GetButtonDown("MoveUp"))
		{
            TryMoveToward(Pos.x, Pos.y + 1);
        }
        if (Input.GetButtonDown("MoveDown"))
		{
            TryMoveToward(Pos.x, Pos.y - 1);
        }
    }

    private void UpdateWorldPosition()
	{
        gameObject.transform.position = new Vector3(Pos.x * TileUtils.TileSize, Pos.y * TileUtils.TileSize, 0.0f);
    }

    private void TryMoveToward(int X, int Y)
	{
        if (CanMoveTo(X, Y))
		{
            Pos.x = X;
            Pos.y = Y;
            UpdateWorldPosition();
		}
        else if (!HasUsedAction && Level.TryInteract(CharType, Pos.x, Pos.y, X, Y))
		{
            HasUsedAction = true;
		}
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
