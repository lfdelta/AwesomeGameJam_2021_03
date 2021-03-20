using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    public Vector2Int RogueSpawn;
    public Vector2Int FighterSpawn;
    public Vector2Int MageSpawn;

    public Dictionary<PlayerCharacterType, Vector2Int> PlayerSpawns;

	void Awake()
	{
		PlayerSpawns = new Dictionary<PlayerCharacterType, Vector2Int>();
		PlayerSpawns[PlayerCharacterType.CT_Rogue] = RogueSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Fighter] = FighterSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Mage] = MageSpawn;
	}
}
