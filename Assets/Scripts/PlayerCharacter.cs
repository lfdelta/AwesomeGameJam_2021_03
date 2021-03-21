using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCharacter : MonoBehaviour
{
    public PlayerCharacterType CharType;

    private Vector2Int Pos;
    private TowerLevel Level;
    private bool IsControlled = false;

    private bool HasUsedAction = false;
    private int MoveDistPerTurn = 0;
    private Vector2Int TurnStartPos;

    private GameObject[] WalkableTileOverlays;
    private bool HasStarted = false;

	void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Texture2D tex;
        switch (CharType)
        {
            case PlayerCharacterType.CT_Rogue:
                MoveDistPerTurn = 3;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Kobold/kobold_5.png", typeof(Texture2D));
                break;
            case PlayerCharacterType.CT_Fighter:
                MoveDistPerTurn = 2;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Goblin/goblin_1.png", typeof(Texture2D));
                break;
            case PlayerCharacterType.CT_Mage:
                MoveDistPerTurn = 2;
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/Character/Ghost/ghost_3.png", typeof(Texture2D));
                break;
            default:
                MoveDistPerTurn = 0;
                tex = null;
                break;
        }
        renderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), tex.width);

        Level = FindObjectOfType<TowerLevel>();
        Pos = Level.PlayerSpawns[CharType];
        TurnStartPos = Pos;
        UpdateWorldPosition();

        HasStarted = true;
        if (IsControlled)
		{
            DisplayWalkableTiles();
		}
    }

    void OnDestroy()
    {
        if (WalkableTileOverlays != null)
        {
            foreach (GameObject obj in WalkableTileOverlays)
            {
                Destroy(obj);
            }
        }
    }

    private void GenerateWalkableTiles()
	{
        VisualTiles.Initialize();
        int nReachableTiles = 1 + 2 * MoveDistPerTurn * (MoveDistPerTurn + 1);
        WalkableTileOverlays = new GameObject[nReachableTiles];
        for (int i = 0; i < nReachableTiles; ++i)
        {
            WalkableTileOverlays[i] = VisualTiles.CreateTileOverlay();
            WalkableTileOverlays[i].SetActive(false);
        }
    }

    private void DisplayWalkableTiles()
	{
        if (WalkableTileOverlays == null)
        {
            GenerateWalkableTiles();
        }
        Color c;
        switch (CharType)
        {
            case PlayerCharacterType.CT_Rogue:
                c = Color.red;
                break;
            case PlayerCharacterType.CT_Fighter:
                c = Color.green;
                break;
            case PlayerCharacterType.CT_Mage:
                c = Color.blue;
                break;
            default:
                c = Color.white;
                break;
        }
        float z = transform.position.z - 0.1f;
        WalkableTileOverlays[0].SetActive(true);
        WalkableTileOverlays[0].GetComponent<SpriteRenderer>().color = c;
        WalkableTileOverlays[0].transform.position = new Vector3(TurnStartPos.x * TileUtils.TileSize, TurnStartPos.y * TileUtils.TileSize, z);
        int i = 1;
        for (int k = 1; k <= MoveDistPerTurn; ++k)
        {
            c.a = 1.0f - (float)k / (float)(MoveDistPerTurn + 1);
            for (int x = k; x > 0; --x)
            {
                int y = k - x;
                if (Level.IsTileTraversable(CharType, TurnStartPos.x + x, TurnStartPos.y + y))
                {
                    WalkableTileOverlays[i].SetActive(true);
                    WalkableTileOverlays[i].GetComponent<SpriteRenderer>().color = c;
                    WalkableTileOverlays[i].transform.position = new Vector3((TurnStartPos.x + x) * TileUtils.TileSize, (TurnStartPos.y + y) * TileUtils.TileSize, z);
                }
                else
				{
                    WalkableTileOverlays[i].SetActive(false);
				}
                ++i;
                if (Level.IsTileTraversable(CharType, TurnStartPos.x + y, TurnStartPos.y - x))
                {
                    WalkableTileOverlays[i].SetActive(true);
                    WalkableTileOverlays[i].GetComponent<SpriteRenderer>().color = c;
                    WalkableTileOverlays[i].transform.position = new Vector3((TurnStartPos.x + y) * TileUtils.TileSize, (TurnStartPos.y - x) * TileUtils.TileSize, z);
                }
                else
                {
                    WalkableTileOverlays[i].SetActive(false);
                }
                ++i;
                if (Level.IsTileTraversable(CharType, TurnStartPos.x - x, TurnStartPos.y - y))
                {
                    WalkableTileOverlays[i].SetActive(true);
                    WalkableTileOverlays[i].GetComponent<SpriteRenderer>().color = c;
                    WalkableTileOverlays[i].transform.position = new Vector3((TurnStartPos.x - x) * TileUtils.TileSize, (TurnStartPos.y - y) * TileUtils.TileSize, z);
                }
                else
                {
                    WalkableTileOverlays[i].SetActive(false);
                }
                ++i;
                if (Level.IsTileTraversable(CharType, TurnStartPos.x - y, TurnStartPos.y + x))
                {
                    WalkableTileOverlays[i].SetActive(true);
                    WalkableTileOverlays[i].GetComponent<SpriteRenderer>().color = c;
                    WalkableTileOverlays[i].transform.position = new Vector3((TurnStartPos.x - y) * TileUtils.TileSize, (TurnStartPos.y + x) * TileUtils.TileSize, z);
                }
                else
                {
                    WalkableTileOverlays[i].SetActive(false);
                }
                ++i;
            }
        }
        if (i != WalkableTileOverlays.Length)
        {
            Debug.LogErrorFormat("Only processed {0} WalkableTileOverlay sprites, expected {1}", i, WalkableTileOverlays.Length);
        }
    }

    public void RemoveControl()
	{
        IsControlled = false;
        foreach (GameObject obj in WalkableTileOverlays)
        {
            obj.SetActive(false);
        }
    }

    public void AcquireControl()
	{
        IsControlled = true;
        if (HasStarted)
        {
            DisplayWalkableTiles();
        }
    }

    public void IncrementTurn()
	{
        TurnStartPos = Pos;
        HasUsedAction = false;
        if (IsControlled)
		{
            DisplayWalkableTiles();
		}
	}

    public Vector2Int GetPosition()
	{
        return Pos;
	}

    void Update()
    {
        if (!IsControlled || HasUsedAction)
        {
            return;
        }

        if (Input.GetButtonDown("MoveLeft"))
		{
            TryMoveToward(Pos.x - 1, Pos.y);
        }
        if (Input.GetButtonDown("MoveRight"))
		{
            TryMoveToward(Pos.x + 1, Pos.y);
        }
        if (Input.GetButtonDown("MoveUp"))
		{
            TryMoveToward(Pos.x, Pos.y + 1);
        }
        if (Input.GetButtonDown("MoveDown"))
		{
            TryMoveToward(Pos.x, Pos.y - 1);
        }
    }

    private void UpdateWorldPosition()
	{
        gameObject.transform.position = new Vector3(Pos.x * TileUtils.TileSize, Pos.y * TileUtils.TileSize, 0.0f);
    }

    private void TryMoveToward(int X, int Y)
	{
        if (CanMoveTo(X, Y))
		{
            Pos.x = X;
            Pos.y = Y;
            UpdateWorldPosition();
		}
        else if (!HasUsedAction && Level.TryInteract(CharType, Pos.x, Pos.y, X, Y))
		{
            HasUsedAction = true;
		}
	}

    private bool CanMoveTo(int X, int Y)
	{
        if (!Level.IsTileTraversable(CharType, X, Y))
		{
            return false;
		}
        return Mathf.Abs(TurnStartPos.x - X) + Mathf.Abs(TurnStartPos.y - Y) <= MoveDistPerTurn;
    }
}
