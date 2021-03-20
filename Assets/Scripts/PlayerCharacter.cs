using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCharacterType CharType;

    private Vector2Int Pos;
    private TowerLevel Level;

    void Start()
    {
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
    }
}
