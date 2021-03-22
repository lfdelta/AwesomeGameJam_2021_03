using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Interactable : MonoBehaviour
{
    public ChangeableTileBlockout AffectedTiles;

    public bool CanRogueInteract;
    public bool CanFighterInteract;
    public bool CanMageInteract;

    public Texture2D DisplayTexture;

    private SpriteRenderer Renderer;

    [HideInInspector]
    public Vector2Int TilePos;

    private bool HasInteracted = false;

    void Awake()
    {
        Transform transf = GetComponent<Transform>();
        TilePos.x = Mathf.FloorToInt(transf.position.x);
        TilePos.y = Mathf.FloorToInt(transf.position.y);
    }

    void Start()
	{
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.transform.localScale = Vector3.one;
        Renderer.sprite = Sprite.Create(DisplayTexture, new Rect(0.0f, 0.0f, DisplayTexture.width, DisplayTexture.height), new Vector2(0.0f, 0.0f), DisplayTexture.width);
        Renderer.color = Color.white;
	}

    public bool TryInteract(PlayerCharacterType CharType)
	{
        if (HasInteracted)
		{
            return false;
		}
        bool succ;
        switch (CharType)
		{
            case PlayerCharacterType.CT_Rogue:
                succ = CanRogueInteract;
                break;
            case PlayerCharacterType.CT_Fighter:
                succ = CanFighterInteract;
                break;
            case PlayerCharacterType.CT_Mage:
                succ = CanMageInteract;
                break;
            default:
                succ = false;
                break;
		}
        if (succ)
		{
            AffectedTiles.SwapTiles();
            HasInteracted = true;
		}
        return succ;
	}
}
