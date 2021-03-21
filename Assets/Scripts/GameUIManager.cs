using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public Canvas BaseCanvas;
	public GameObject GameOverRoot;
	public Text GameOverText;

	private float AnimateInTime = 1.0f;

	void Start()
	{
		GameOverRoot.SetActive(false);
	}

    public void StartGameOver(string Reason)
	{
		GameOverRoot.SetActive(true);
		GameOverText.text = string.Format("Game over!\n{0}", Reason);
		StartCoroutine(AnimateInGameOver());
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
