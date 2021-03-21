using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public GameObject GameOverRoot;
	public Text GameOverText;
	public GameObject MenuRoot;

	public GameObject HUDRoot;
	public Text TurnCounter;
	public Text CharacterText;

	private float AnimateInTime = 1.0f;

	public void EnterMainMenu()
	{
		ExitGameOver();
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

	public void ExitGameOver()
	{
		StopCoroutine(AnimateInGameOver());
		GameOverRoot.SetActive(false);
	}

	public void EnterGameplay()
	{
		ExitGameOver();
		ExitMainMenu();
	}

	public void SetTurn(int Turn, int Max)
	{
		TurnCounter.text = string.Format("Turn: {0}/{1}", Turn, Max);
	}

	public void SetControlledCharacter(PlayerCharacter Character)
	{
		CharacterText.text = string.Format("{0}: {1}",
			PlayerCharacterUtils.GetTypeString(Character.CharType),
			Character.GetHasUsedAction() ? "Action used" : "Action available");
		CharacterText.color = PlayerCharacterUtils.GetTypeColor(Character.CharType);
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
