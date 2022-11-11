using UnityEngine;
using TMPro;

public static class GridDebugger
{
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, float charSize, Color color, TextAnchor textAncor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_text_G.D.", typeof(TextMesh));

        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAncor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.characterSize = charSize;

        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }
}
