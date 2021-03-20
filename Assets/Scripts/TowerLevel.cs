using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    public Vector2Int RogueSpawn;
    public Vector2Int FighterSpawn;
    public Vector2Int MageSpawn;

    public Dictionary<PlayerCharacterType, Vector2Int> PlayerSpawns;
	private TileType[,] LevelTiles;
	private Vector2Int TileOrigin;
	private Vector2Int LevelTileSize;

	void Awake()
	{
		PlayerSpawns = new Dictionary<PlayerCharacterType, Vector2Int>();
		PlayerSpawns[PlayerCharacterType.CT_Rogue] = RogueSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Fighter] = FighterSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Mage] = MageSpawn;
	}

	void Start()
	{
		TileBlockoutVolume[] blockouts = FindObjectsOfType<TileBlockoutVolume>();
		Vector2Int min = blockouts[0].MinTile;
		Vector2Int max = blockouts[0].MaxTile;
		foreach (TileBlockoutVolume vol in blockouts)
		{
			if (vol.MinTile.x < min.x)
			{
				min.x = vol.MinTile.x;
			}
			if (vol.MinTile.y < min.y)
			{
				min.y = vol.MinTile.y;
			}
			if (vol.MaxTile.x > max.x)
			{
				max.x = vol.MaxTile.x;
			}
			if (vol.MaxTile.y > max.y)
			{
				max.y = vol.MaxTile.y;
			}
		}
		TileOrigin = min;
		LevelTileSize.x = max.x - min.x + 1;
		LevelTileSize.y = max.y - min.y + 1;
		LevelTiles = new TileType[LevelTileSize.x, LevelTileSize.y];

		Debug.LogFormat("World min [{0}, {1}], max [{2}, {3}]", min.x, min.y, max.x, max.y);
		Debug.LogFormat("LevelTileSize [{0}, {1}]", LevelTileSize.x, LevelTileSize.y);

		for (int x = 0; x < LevelTileSize.x; ++x)
		{
			for (int y = 0; y < LevelTileSize.y; ++y)
			{
				LevelTiles[x, y] = TileType.TT_Undefined;
			}
		}
		foreach(TileBlockoutVolume vol in blockouts)
		{
			for (int x = vol.MinTile.x; x <= vol.MaxTile.x; ++x)
			{
				for (int y = vol.MinTile.y; y <= vol.MaxTile.y; ++y)
				{
					if (vol.Type > LevelTiles[x - TileOrigin.x, y - TileOrigin.y])
					{
						Debug.LogFormat("[{0}, {1}]", x, y);
						LevelTiles[x - TileOrigin.x, y - TileOrigin.y] = vol.Type;
					}
				}
			}
		}
	}

	public bool IsTileTraversable(PlayerCharacterType Character, int X, int Y)
	{
		if (X < TileOrigin.x || Y < TileOrigin.y ||
			X >= TileOrigin.x + LevelTileSize.x || Y >= TileOrigin.y + LevelTileSize.y)
		{
			return false;
		}
		TileType t = LevelTiles[X - TileOrigin.x, Y - TileOrigin.y];
		switch (t)
		{
			case TileType.TT_Wall:
			case TileType.TT_Door:
			case TileType.TT_Breakable:
				return false;
			case TileType.TT_Open:
				return true;
			case TileType.TT_MagePassable:
				return Character == PlayerCharacterType.CT_Mage;
			default:
				Debug.LogErrorFormat("IsTileValid found unexpected tile {} at [{}, {}]", t.ToString(), X, Y);
				return false;
		}
	}
}