using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
	private Dictionary<PlayerCharacterType, GameObject> PlayerChars;
	private PlayerCharacterType ControlledChar;

	void Awake()
	{
		GameObject playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObjects/PlayerCharacter.prefab", typeof(GameObject));
		PlayerChars = new Dictionary<PlayerCharacterType, GameObject>();

		GameObject rogue = Instantiate(playerPrefab);
		rogue.GetComponent<PlayerCharacter>().CharType = PlayerCharacterType.CT_Rogue;
		PlayerChars[PlayerCharacterType.CT_Rogue] = rogue;

		GameObject fighter = Instantiate(playerPrefab);
		fighter.GetComponent<PlayerCharacter>().CharType = PlayerCharacterType.CT_Fighter;
		PlayerChars[PlayerCharacterType.CT_Fighter] = fighter;

		GameObject mage = Instantiate(playerPrefab);
		mage.GetComponent<PlayerCharacter>().CharType = PlayerCharacterType.CT_Mage;
		PlayerChars[PlayerCharacterType.CT_Mage] = mage;

		ControlledChar = PlayerCharacterType.CT_Rogue;
		PlayerChars[ControlledChar].GetComponent<PlayerCharacter>().AcquireControl();
	}

	void OnDestroy()
	{
		foreach (GameObject obj in PlayerChars.Values)
		{
			Destroy(obj);
		}
	}

	void Update()
	{
		if (GameFlowManager.GetIsGameOver())
		{
			return;
		}
		if (Input.GetButtonDown("SwitchRogue") && ControlledChar != PlayerCharacterType.CT_Rogue)
		{
			ChangeCharacter(PlayerCharacterType.CT_Rogue);
		}
		if (Input.GetButtonDown("SwitchFighter") && ControlledChar != PlayerCharacterType.CT_Fighter)
		{
			ChangeCharacter(PlayerCharacterType.CT_Fighter);
		}
		if (Input.GetButtonDown("SwitchMage") && ControlledChar != PlayerCharacterType.CT_Mage)
		{
			ChangeCharacter(PlayerCharacterType.CT_Mage);
		}
		if (Input.GetButtonDown("PrevCharacter"))
		{
			switch (ControlledChar)
			{
				case PlayerCharacterType.CT_Rogue:
					ChangeCharacter(PlayerCharacterType.CT_Mage);
					break;
				case PlayerCharacterType.CT_Fighter:
					ChangeCharacter(PlayerCharacterType.CT_Rogue);
					break;
				case PlayerCharacterType.CT_Mage:
					ChangeCharacter(PlayerCharacterType.CT_Fighter);
					break;
			}
		}
		if (Input.GetButtonDown("NextCharacter"))
		{
			switch (ControlledChar)
			{
				case PlayerCharacterType.CT_Rogue:
					ChangeCharacter(PlayerCharacterType.CT_Fighter);
					break;
				case PlayerCharacterType.CT_Fighter:
					ChangeCharacter(PlayerCharacterType.CT_Mage);
					break;
				case PlayerCharacterType.CT_Mage:
					ChangeCharacter(PlayerCharacterType.CT_Rogue);
					break;
			}
		}
	}

	private void ChangeCharacter(PlayerCharacterType NewType)
	{
		PlayerChars[ControlledChar].GetComponent<PlayerCharacter>().RemoveControl();
		PlayerChars[NewType].GetComponent<PlayerCharacter>().AcquireControl();
		ControlledChar = NewType;
	}

	public GameObject GetControlledCharacter()
	{
		return PlayerChars[ControlledChar];
	}

	public PlayerCharacter[] GetAllCharacters()
	{
		PlayerCharacter[] ret = new PlayerCharacter[PlayerChars.Keys.Count];
		int i = 0;
		foreach (GameObject pc in PlayerChars.Values)
		{
			ret[i++] = pc.GetComponent<PlayerCharacter>();
		}
		return ret;
	}
}
