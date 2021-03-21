using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public string[] TowerLevels;

    private int CurrLevel = 0;

    private AsyncOperation AsyncSceneOp;

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
        AsyncSceneOp = SceneManager.UnloadSceneAsync(TowerLevels[CurrLevel]);
        StartCoroutine(WaitToReturnToMenu());
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
            AsyncSceneOp = SceneManager.UnloadSceneAsync(TowerLevels[CurrLevel - 1]);
            StartCoroutine(WaitToLoadNextLevel());
		}
	}

    private IEnumerator WaitToLoadNextLevel()
	{
        // TODO: loading screen?
        while (!AsyncSceneOp.isDone)
		{
            yield return null;
		}
        AsyncSceneOp = SceneManager.LoadSceneAsync(TowerLevels[CurrLevel], LoadSceneMode.Additive);
		while (!AsyncSceneOp.isDone)
		{
            yield return null;
		}
	}

    private IEnumerator WaitToReturnToMenu()
	{
        while (!AsyncSceneOp.isDone)
        {
            yield return null;
        }
        // TODO: load the menu scene
    }

    private void GameWon()
	{
        // TODO: actual display
        Debug.Log("YOU WON THE GAME!!! NICE WORK");

        AsyncSceneOp = SceneManager.UnloadSceneAsync(TowerLevels[TowerLevels.Length - 1]);
        StartCoroutine(WaitToReturnToMenu());
	}

    public void GameOver(string Reason)
	{
        // TODO
        Debug.LogFormat("GAME OVER!!! {0}", Reason);
    }
}
