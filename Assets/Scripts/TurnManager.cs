using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int TurnCount = 1;
    private TowerLevel Level;
    private PlayerCharacter[] PlayerChars;

    void Start()
    {
        Level = FindObjectOfType<TowerLevel>();
        PlayerManager playerMgr = FindObjectOfType<PlayerManager>();
        PlayerChars = playerMgr.GetAllCharacters();
    }

    void Update()
	{
        if (Input.GetButtonDown("CompleteTurn"))
		{
            Debug.Log("Ended turn " + TurnCount);
            ++TurnCount;
            foreach(PlayerCharacter pc in PlayerChars)
			{
                pc.IncrementTurn();
			}
            Level.IncrementTurn(TurnCount - 1);
		}
	}

    public int GetTurnCount()
	{
        return TurnCount;
	}
}
