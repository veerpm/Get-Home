using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public SpriteRenderer bgSprite; // background
    public TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        // set text in front of background
        textMesh.sortingLayerID = SortingLayer.NameToID("Dialogue");
        textMesh.sortingOrder = 10;
    }

    public void Setup(string text, float size = 1.25f)
    {
        // Change text
        textMesh.SetText(text);
        textMesh.fontSize = size;
        textMesh.ForceMeshUpdate();

        // update background for text
        Vector2 textSize = textMesh.GetRenderedValues(false);
        Vector2 padding = new Vector2(1.05f, 1.025f);
        // actual change
        float newScaleX = (float) (textSize.x *padding.x) / (float) bgSprite.sprite.bounds.size.x;
        float newScaleY = (textSize.y*padding.y) / bgSprite.sprite.bounds.size.y;
        //bgSprite.size = textSize + padding;
        bgSprite.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);
        bgSprite.transform.localPosition = new Vector3(0f, 0f);

    }
}
