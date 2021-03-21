using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    public Vector2Int RogueSpawn;
    public Vector2Int FighterSpawn;
    public Vector2Int MageSpawn;

	public int AllowedTurns;

    public Dictionary<PlayerCharacterType, Vector2Int> PlayerSpawns;
	private TileType[,] LevelTiles;
	private GameObject[,] TileObjects;
	private Vector2Int TileOrigin;
	private Vector2Int LevelTileSize;

	private PlayerManager PlayerMgr;
	private Dictionary<int, List<Vector2Int>> FallingRocks;
	private Dictionary<Vector2Int, Vector2Int> DoorUnlockDirs;
	private Dictionary<Vector2Int, Interactable> InteractableTiles;

	void Awake()
	{
		PlayerSpawns = new Dictionary<PlayerCharacterType, Vector2Int>();
		PlayerSpawns[PlayerCharacterType.CT_Rogue] = RogueSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Fighter] = FighterSpawn;
		PlayerSpawns[PlayerCharacterType.CT_Mage] = MageSpawn;

		PlayerMgr = gameObject.AddComponent<PlayerManager>();
		gameObject.AddComponent<TurnManager>();
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
		TileObjects = new GameObject[LevelTileSize.x, LevelTileSize.y];

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

		// Gather and seed rocks
		FallingRocks = new Dictionary<int, List<Vector2Int>>();
		RockBlockout[] rocks = FindObjectsOfType<RockBlockout>();
		foreach (RockBlockout r in rocks)
		{
			if (r.FallTurn < 1)
			{
				LevelTiles[r.TilePos.x - TileOrigin.x, r.TilePos.y - TileOrigin.y] = TileType.TT_Breakable;
			}
			else
			{
				if (FallingRocks.ContainsKey(r.FallTurn))
				{
					FallingRocks[r.FallTurn].Add(r.TilePos);
				}
				else
				{
					List<Vector2Int> collxn = new List<Vector2Int>();
					collxn.Add(r.TilePos);
					FallingRocks[r.FallTurn] = collxn;
				}
			}
		}

		// Gather and seed doors
		DoorUnlockDirs = new Dictionary<Vector2Int, Vector2Int>();
		DoorBlockout[] doors = FindObjectsOfType<DoorBlockout>();
		foreach (DoorBlockout d in doors)
		{
			LevelTiles[d.TilePos.x - TileOrigin.x, d.TilePos.y - TileOrigin.y] = TileType.TT_Door;
			Vector2Int unlockFrom = new Vector2Int(0, 0);
			switch(d.UnlockDirection)
			{
				case 0:
					unlockFrom.x = 1;
					break;
				case 1:
					unlockFrom.y = 1;
					break;
				case 2:
					unlockFrom.x = -1;
					break;
				case 3:
					unlockFrom.y = -1;
					break;
				default:
					Debug.LogErrorFormat("Found door at [{0}, {1}] with invalid UnlockDirection {2}", d.TilePos.x, d.TilePos.y, d.UnlockDirection);
					break;
			}
			DoorUnlockDirs.Add(d.TilePos, d.TilePos + unlockFrom);
		}

		InteractableTiles = new Dictionary<Vector2Int, Interactable>();
		Interactable[] interactables = FindObjectsOfType<Interactable>();
		foreach (Interactable ixn in interactables)
		{
			InteractableTiles.Add(ixn.TilePos, ixn);
		}
		foreach (ChangeableTileBlockout c in FindObjectsOfType<ChangeableTileBlockout>())
		{
			for (int x = c.MinTile.x; x <= c.MaxTile.x; ++x)
			{
				for (int y = c.MinTile.y; y <= c.MaxTile.y; ++y)
				{
					LevelTiles[x, y] = c.StartType;
				}
			}
		}

		// Generate visual tiles based on the logical tiles
		VisualTiles.Initialize();
		for (int x = 0; x < LevelTileSize.x; ++x)
		{
			for (int y = 0; y < LevelTileSize.y; ++y)
			{
				if (LevelTiles[x, y] != TileType.TT_Undefined)
				{
					TileObjects[x, y] = VisualTiles.CreateTile(LevelTiles[x, y], TileOrigin.x + x, TileOrigin.y + y);
				}
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

	void OnDestroy()
	{
		foreach (GameObject obj in TileObjects)
		{
			Destroy(obj);
		}
	}

	public bool IsTileTraversable(PlayerCharacterType Character, int X, int Y)
	{
		if (X < TileOrigin.x || Y < TileOrigin.y ||
			X >= TileOrigin.x + LevelTileSize.x || Y >= TileOrigin.y + LevelTileSize.y)
		{
			return false;
		}
		if (InteractableTiles.ContainsKey(new Vector2Int(X, Y)))
		{
			return false;
		}
		TileType t = LevelTiles[X - TileOrigin.x, Y - TileOrigin.y];
		switch (t)
		{
			case TileType.TT_Undefined:
			case TileType.TT_Wall:
			case TileType.TT_Door:
			case TileType.TT_Breakable:
				return false;
			case TileType.TT_Open:
			case TileType.TT_LevelEnd:
				return true;
			case TileType.TT_MagePassable:
				return Character == PlayerCharacterType.CT_Mage;
			default:
				Debug.LogErrorFormat("IsTileValid found unexpected tile {0} at [{1}, {2}]", t.ToString(), X, Y);
				return false;
		}
	}

	public TileType GetTileType(Vector2Int Pos)
	{
		if (Pos.x < TileOrigin.x || Pos.y < TileOrigin.y ||
			Pos.x >= TileOrigin.x + LevelTileSize.x || Pos.y >= TileOrigin.y + LevelTileSize.y)
		{
			return TileType.TT_Undefined;
		}
		return LevelTiles[Pos.x - TileOrigin.x, Pos.y - TileOrigin.y];
	}

	public bool TryInteract(PlayerCharacterType Character, int StartX, int StartY, int EndX, int EndY)
	{
		if (EndX < TileOrigin.x || EndY < TileOrigin.y ||
			EndX >= TileOrigin.x + LevelTileSize.x || EndY >= TileOrigin.y + LevelTileSize.y)
		{
			return false;
		}
		Vector2Int endPos = new Vector2Int(EndX, EndY);
		if (InteractableTiles.ContainsKey(endPos))
		{
			if (InteractableTiles[endPos].TryInteract(Character))
			{
				return true;
			}
		}
		TileType t = LevelTiles[EndX - TileOrigin.x, EndY - TileOrigin.y];
		switch (t)
		{
			case TileType.TT_Door:
				if (Character == PlayerCharacterType.CT_Rogue || DoorUnlockDirs[endPos] == new Vector2Int(StartX, StartY))
				{
					ReplaceTileObject(TileType.TT_Open, EndX, EndY);
					return true;
				}
				return false;
			case TileType.TT_Breakable:
				if (Character == PlayerCharacterType.CT_Fighter)
				{
					ReplaceTileObject(TileType.TT_Open, EndX, EndY);
					return true;
				}
				return false;
			case TileType.TT_Wall:
			case TileType.TT_Open:
			case TileType.TT_MagePassable:
			case TileType.TT_Undefined:
			case TileType.TT_LevelEnd:
				return false;
			default:
				Debug.LogErrorFormat("TryInteract found unexpected tile {0} at [{1}, {2}]", t.ToString(), EndX, EndY);
				return false;
		}
	}

	public void IncrementTurn(int CompletedTurnNumber)
	{
		// Check collisions, then update tiles, for falling rocks
		if (FallingRocks.ContainsKey(CompletedTurnNumber))
		{
			foreach (Vector2Int rockPos in FallingRocks[CompletedTurnNumber])
			{
				ReplaceTileObject(TileType.TT_Breakable, rockPos.x - TileOrigin.x, rockPos.y - TileOrigin.y);
			}
			foreach (PlayerCharacter pc in PlayerMgr.GetAllCharacters())
			{
				Vector2Int playerPos = pc.GetPosition();
				foreach (Vector2Int rockPos in FallingRocks[CompletedTurnNumber])
				{
					if (rockPos == playerPos)
					{
						FindObjectOfType<GameFlowManager>().GameOver(string.Format("{0} was hit by a falling rock!",
							PlayerCharacterUtils.GetTypeString(pc.CharType)));
						return;
					}
				}
			}
		}

		if (CompletedTurnNumber >= AllowedTurns)
		{
			FindObjectOfType<GameFlowManager>().GameOver("The tower crumbled before you could escape!");
		}
	}

	private void ReplaceTileObject(TileType Type, int X, int Y)
	{
		LevelTiles[X, Y] = Type;
		Destroy(TileObjects[X, Y]);
		TileObjects[X, Y] = VisualTiles.CreateTile(Type, X, Y);
	}

	public void ReplaceTiles(TileType Type, int StartX, int StartY, int EndX, int EndY)
	{
		for (int x = StartX; x <= EndX; ++x)
		{
			for (int y = StartY; y <= EndY; ++y)
			{
				ReplaceTileObject(Type, x, y);
			}
		}
	}
}
