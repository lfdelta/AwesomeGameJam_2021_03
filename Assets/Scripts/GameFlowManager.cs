using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public string[] TowerLevels;
    public GameUIManager UIManager;

    private int CurrLevel = 0;
    private bool IsGameOver = false;

    public static bool GetIsGameOver()
	{
        return FindObjectOfType<GameFlowManager>().IsGameOver;
	}

    void Start()
	{
        UIManager = FindObjectOfType<GameUIManager>();
        UIManager.EnterMainMenu();
    }

    public void EnterGameFromMenu()
	{
        IsGameOver = false;
        UIManager.EndGameOver();
        UIManager.ExitMainMenu();
        CurrLevel = 0;
        SceneManager.LoadScene(TowerLevels[0], LoadSceneMode.Additive);
    }

    public void ReturnToMenu()
	{
        IsGameOver = false;
        UIManager.EnterMainMenu();
        SceneManager.UnloadSceneAsync(TowerLevels[CurrLevel]);
    }

    public void MoveToNextLevel()
	{
        ++CurrLevel;
        if (CurrLevel >= TowerLevels.Length)
		{
            GameWon();
		}
        else
		{
            StartCoroutine(SwapLevel(TowerLevels[CurrLevel - 1], TowerLevels[CurrLevel]));
		}
	}

    public void RestartLevel()
	{
        IsGameOver = false;
        UIManager.EndGameOver();
        StartCoroutine(SwapLevel(TowerLevels[CurrLevel], TowerLevels[CurrLevel]));
	}

    private IEnumerator SwapLevel(string Old, string New)
	{
        // TODO: loading screen/transition
        AsyncOperation asyncSceneOp = SceneManager.UnloadSceneAsync(Old);
        while (!asyncSceneOp.isDone)
		{
            yield return null;
		}
        SceneManager.LoadSceneAsync(New, LoadSceneMode.Additive);
    }

    private void GameWon()
	{
        // TODO: actual display
        Debug.Log("YOU WON THE GAME!!! NICE WORK");

        --CurrLevel; // Unload the right scene
        ReturnToMenu();
	}

    public void GameOver(string Reason)
	{
        IsGameOver = true;
        UIManager.StartGameOver(Reason);
    }
}
