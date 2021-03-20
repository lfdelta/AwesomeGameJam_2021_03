using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int TurnCount = 0;

    private PlayerCharacter[] PlayerChars;

    void Start()
    {
        PlayerManager playerMgr = FindObjectOfType<PlayerManager>();
        PlayerChars = playerMgr.GetAllCharacters();
    }

    void Update()
	{
        if (Input.GetButtonDown("CompleteTurn"))
		{
            ++TurnCount;
            foreach(PlayerCharacter pc in PlayerChars)
			{
                pc.IncrementTurn();
			}
            // TODO: falling rocks
		}
	}
}
