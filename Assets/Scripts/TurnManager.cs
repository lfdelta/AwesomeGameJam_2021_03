using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int TurnCount = 1;
    private TowerLevel Level;
    private PlayerCharacter[] PlayerChars;
    private GameUIManager UIManager;

    void Start()
    {
        Level = FindObjectOfType<TowerLevel>();
        PlayerManager playerMgr = FindObjectOfType<PlayerManager>();
        PlayerChars = playerMgr.GetAllCharacters();
        UIManager = FindObjectOfType<GameUIManager>();
        UIManager.SetTurn(TurnCount, Level.AllowedTurns);
    }

    void Update()
	{
        if (GameFlowManager.GetIsGameOver())
		{
            return;
		}
        if (Input.GetButtonDown("CompleteTurn"))
		{
            Debug.Log("Ended turn " + TurnCount);

            bool isWon = true;
            foreach (PlayerCharacter pc in PlayerChars)
			{
                if (Level.GetTileType(pc.GetPosition()) != TileType.TT_LevelEnd)
				{
                    isWon = false;
                    break;
				}
			}
            if (isWon)
            {
                FindObjectOfType<GameFlowManager>().MoveToNextLevel();
            }
            else
            {
                ++TurnCount;
                UIManager.SetTurn(TurnCount, Level.AllowedTurns);
                Level.IncrementTurn(TurnCount - 1);
                foreach (PlayerCharacter pc in PlayerChars)
                {
                    pc.IncrementTurn();
                }
            }
        }
	}

    public int GetTurnCount()
	{
        return TurnCount;
	}
}
