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
        // first text
        //Setup("Looooooooooooooooooooooooong Dialogue");
        Setup("Dialogue");
    }

    private void Setup(string text)
    {
        textMesh.SetText(text);
        textMesh.ForceMeshUpdate();

        // update background for text
        Vector2 textSize = textMesh.GetRenderedValues(false);
        Vector2 padding = new Vector2(1f, 0.5f);
        // actual change
        float newScaleX = (textSize.x + padding.x) / bgSprite.sprite.bounds.size.x;
        float newScaleY = (textSize.y + padding.y) / bgSprite.sprite.bounds.size.y;
        //bgSprite.size = textSize + padding;
        bgSprite.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);
        bgSprite.transform.localPosition = new Vector3(0f, 0f);

    }
}
