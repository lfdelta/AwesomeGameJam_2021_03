using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(TextMesh))]
public class RockBlockout : MonoBehaviour
{
    // Falls at the END of this turn.
    public int FallTurn = 0;

    [HideInInspector]
    public Vector2Int TilePos;

    void Awake()
    {
        Transform transf = GetComponent<Transform>();
        TilePos.x = Mathf.FloorToInt(transf.position.x);
        TilePos.y = Mathf.FloorToInt(transf.position.y);

        UpdateTurnPreview();
    }

	void OnValidate()
	{
        UpdateTurnPreview();
	}

    private void UpdateTurnPreview()
	{
        TextMesh previewText = GetComponent<TextMesh>();
        previewText.text = string.Format(" {0}", FallTurn);
        previewText.color = Color.red;
        previewText.anchor = TextAnchor.LowerLeft;
        previewText.fontSize = 16;
        previewText.richText = false;
    }
}
