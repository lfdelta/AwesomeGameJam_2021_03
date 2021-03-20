using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCharacterType CharType;

    private Vector2Int Pos;

    void Start()
    {
        TowerLevel lvl = FindObjectOfType<TowerLevel>();
        Pos = lvl.PlayerSpawns[CharType];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
		{
            --Pos.x;
            Debug.Log("Position: " + Pos.x + ", " + Pos.y);
		}
        if (Input.GetKeyDown(KeyCode.D))
		{
            ++Pos.x;
            Debug.Log("Position: " + Pos.x + ", " + Pos.y);
        }
        if (Input.GetKeyDown(KeyCode.W))
		{
            ++Pos.y;
            Debug.Log("Position: " + Pos.x + ", " + Pos.y);
        }
        if (Input.GetKeyDown(KeyCode.S))
		{
            --Pos.y;
            Debug.Log("Position: " + Pos.x + ", " + Pos.y);
        }
    }
}
