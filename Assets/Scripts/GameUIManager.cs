using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public Canvas BaseCanvas;
	public GameObject GameOverRoot;
	public Text GameOverText;
	public GameObject MenuRoot;

	private float AnimateInTime = 1.0f;

	public void EnterMainMenu()
	{
		GameOverRoot.SetActive(false);
		MenuRoot.SetActive(true);
	}

	public void ExitMainMenu()
	{
		MenuRoot.SetActive(false);
	}

    public void StartGameOver(string Reason)
	{
		GameOverRoot.SetActive(true);
		GameOverText.text = Reason;
		StartCoroutine(AnimateInGameOver());
	}

	public void EndGameOver()
	{
		GameOverRoot.SetActive(false);
	}

	private IEnumerator AnimateInGameOver()
	{
		float animTime = 0.0f;
		while (animTime < AnimateInTime)
		{
			animTime += Time.deltaTime;
			SetAlpha(animTime / AnimateInTime);
			yield return null;
		}
		SetAlpha(1.0f);
	}

	private void SetAlpha(float InterpVal)
	{
		GameOverRoot.GetComponent<CanvasGroup>().alpha = Mathf.Sqrt(InterpVal);
	}
}
