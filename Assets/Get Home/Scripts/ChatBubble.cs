using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public SpriteRenderer bgSprite; // background
    public TextMeshPro textMesh;

    private bool inversed = false;

    // Start is called before the first frame update
    void Start()
    {
        // set text in front of background
        textMesh.sortingLayerID = SortingLayer.NameToID("Dialogue");
        textMesh.sortingOrder = 10;
        bgSprite.sortingLayerID = SortingLayer.NameToID("Dialogue");
        bgSprite.sortingOrder = 9;
    }

    private void Update()
    {
        // inverse picture if parent is inversed
        if (transform.parent.transform.localScale.x < 0 && !inversed)
        {
            inversed = true;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else if (transform.parent.transform.localScale.x > 0 && inversed)
        {
            inversed = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

    }
    public void Setup(string text, float size = 1f)
    {

        // Change text
        textMesh.SetText(text);
        textMesh.fontSize = size;
        textMesh.ForceMeshUpdate();

        // update background for text
        Vector2 textSize = textMesh.GetRenderedValues(false);
        // padding is in PERCENTAGE
        // Vector2 padding = new Vector2(0.05f, 0.025f); // ancient chatBubble's offset
        Vector2 padding = new Vector2(0.1f, 0.7f); // new chatBubbles' offset
        // actual change
        float newScaleX = (float) (1f+padding.x)* (textSize.x) / (float)bgSprite.sprite.bounds.size.x;
        float newScaleY = (float) (1f+padding.y)*(textSize.y) / bgSprite.sprite.bounds.size.y;
        //bgSprite.size = textSize + padding;
        bgSprite.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);
        bgSprite.transform.localPosition = new Vector3(0f, 0f);

    }
}
