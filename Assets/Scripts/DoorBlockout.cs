using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(TextMesh))]
public class DoorBlockout : MonoBehaviour
{
    // 0 = From the right, 1 = From above, 2 = From the left, 3 = From below
    public int UnlockDirection = 0;

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
        char c;
        switch (UnlockDirection)
		{
            case 0:
                c = '<';
                break;
            case 1:
                c = 'v';
                break;
            case 2:
                c = '>';
                break;
            case 3:
                c = '^';
                break;
            default:
                c = 'x';
                break;
		}
        previewText.text = string.Format(" {0}", c);
        previewText.color = Color.red;
        previewText.anchor = TextAnchor.LowerLeft;
        previewText.fontSize = 16;
        previewText.richText = false;
    }
}
