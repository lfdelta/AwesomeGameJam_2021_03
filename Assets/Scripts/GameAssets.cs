using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Texture2D Rogue;
    public Texture2D Fighter;
    public Texture2D Mage;

    public Texture2D[] FloorTiles;
    public Texture2D[] WallTiles;
    public Texture2D[] BreakableTiles;
    public Texture2D[] MagePassableTiles;
    public Texture2D[] DoorTiles;
    public Texture2D[] LevelEndTiles;

    public Texture2D WalkableTileOverlay;

    public GameObject TilePrefab;
    public GameObject PlayerPrefab;
}
