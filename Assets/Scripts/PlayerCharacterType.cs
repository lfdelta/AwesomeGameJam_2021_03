using UnityEngine;

public enum PlayerCharacterType
{
	CT_Rogue,
	CT_Fighter,
	CT_Mage
}

public static class PlayerCharacterUtils
{
	public static string GetTypeString(PlayerCharacterType Type)
	{
		switch (Type)
		{
			case PlayerCharacterType.CT_Rogue:
				return "Kobold";
			case PlayerCharacterType.CT_Fighter:
				return "Goblin";
			case PlayerCharacterType.CT_Mage:
				return "Ghost";
			default:
				return "INVALID";
		}
	}

	public static Color GetTypeColor(PlayerCharacterType Type)
	{
		switch (Type)
		{
			case PlayerCharacterType.CT_Rogue:
				return Color.red;
			case PlayerCharacterType.CT_Fighter:
				return Color.green;
			case PlayerCharacterType.CT_Mage:
				return Color.blue;
			default:
				return Color.white;
		}
	}
}