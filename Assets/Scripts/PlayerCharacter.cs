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
    }
}
