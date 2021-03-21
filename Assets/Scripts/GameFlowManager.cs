using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public string MenuScene;
    public string[] TowerLevels;

    private int CurrLevel = 0;

    void Awake()
    {
        // TODO: set up menu screen
        EnterGameFromMenu();
    }

    public void EnterGameFromMenu()
	{
        CurrLevel = 0;
        SceneManager.LoadScene(TowerLevels[0], LoadSceneMode.Additive);
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
    }

    private void GameWon()
	{
        // TODO: actual display
        Debug.Log("YOU WON THE GAME!!! NICE WORK");

        StartCoroutine(SwapLevel(TowerLevels[TowerLevels.Length - 1], MenuScene));
	}

    public void GameOver(string Reason)
	{
        // TODO: actual display
        Debug.LogFormat("GAME OVER!!! {0}", Reason);
    }
}
