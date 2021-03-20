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

		gameObject.AddComponent<PlayerManager>();
	}

	void Start()
	{
		// Find bounds of all blockout volumes
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

		// Fill in the logical tile state based on blockout volumes
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
						LevelTiles[x - TileOrigin.x, y - TileOrigin.y] = vol.Type;
					}
				}
			}
		}

		// Generate visual tiles based on the logical tiles
		VisualTiles.Initialize();
		for (int x = 0; x < LevelTileSize.x; ++x)
		{
			for (int y = 0; y < LevelTileSize.y; ++y)
			{
				VisualTiles.CreateTile(LevelTiles[x, y], TileOrigin.x + x, TileOrigin.y + y);
			}
		}

		// Set camera to view everything at once
		float horizSize = (max.x - min.x);
		float vertSize = (max.y - min.y);
		float camBuffer = TileUtils.TileSize;
		float camVertSize = Mathf.Max(vertSize, horizSize / Camera.main.aspect) / 2.0f;
		Camera.main.orthographicSize = camVertSize + camBuffer;

		Camera.main.transform.position = new Vector3(
			((max.x + min.x) * TileUtils.TileSize + camBuffer)/ 2.0f,
			((max.y + min.y) * TileUtils.TileSize + camBuffer) / 2.0f,
			-10.0f);
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
