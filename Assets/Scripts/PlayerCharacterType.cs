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
}