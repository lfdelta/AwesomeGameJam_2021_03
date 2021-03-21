using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public string MenuScene;
    public string[] TowerLevels;
    public GameUIManager UIManager;

    private int CurrLevel = 0;

    void Start()
	{
        // TODO: set up menu screen
        UIManager = FindObjectOfType<GameUIManager>();
        EnterGameFromMenu();
    }

    public void EnterGameFromMenu()
	{
        CurrLevel = 0;
        SceneManager.LoadScene(TowerLevels[0], LoadSceneMode.Additive);
        UIManager.BaseCanvas.worldCamera = Camera.main;
        // TODO: any intro stuff
	}

    public void ReturnToMenu()
	{
        StartCoroutine(SwapLevel(TowerLevels[CurrLevel], MenuScene));
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

    private IEnumerator SwapLevel(string Old, string New)
	{
        // TODO: loading screen
        AsyncOperation asyncSceneOp = SceneManager.UnloadSceneAsync(Old);
        while (!asyncSceneOp.isDone)
		{
            yield return null;
		}
        SceneManager.LoadSceneAsync(New, LoadSceneMode.Additive);
        while (!asyncSceneOp.isDone)
		{
            yield return null;
		}
        UIManager.BaseCanvas.worldCamera = Camera.main;
    }

    private void GameWon()
	{
        // TODO: actual display
        Debug.Log("YOU WON THE GAME!!! NICE WORK");

        StartCoroutine(SwapLevel(TowerLevels[TowerLevels.Length - 1], MenuScene));
	}

    public void GameOver(string Reason)
	{
        UIManager.StartGameOver(Reason);
    }
}
