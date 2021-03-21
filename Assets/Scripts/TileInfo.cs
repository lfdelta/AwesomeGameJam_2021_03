public enum TileType
{
	// Defined in order of increasing priority (latter will override the former)
	TT_Undefined = 0,
	TT_Open = 1,
	TT_Wall = 2,
	TT_Door = 3,
	TT_Breakable = 4,
	TT_MagePassable = 5,
	TT_LevelEnd = 6,
}

abstract class TileUtils
{
	public static readonly float TileSize = 1.0f;
}