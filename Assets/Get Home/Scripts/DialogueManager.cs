using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject chatObject;

    // Variable player and Start method are only there for TESTING purposes (calling the method Create()
    // is simpler if you want to create dialogues).
    public GameObject player;

    public void Start()
    {
        Create(player, "Hi! I can talk through chat bubbles now!", 3f);
    }
 

    // Params = original, Vector3 position, Quaternion rotation, Transform parent
    public void Create(GameObject parent, string text, float time, Vector3 localPosition = default(Vector3))
    {
        // move slightly above head if no pos. given
        if(localPosition == default(Vector3))
        {
            localPosition = new Vector3(0, 1f, 0);
        }

        // create & set dialogue
        GameObject chatBubble = Instantiate(chatObject, parent.transform.position+localPosition, 
            Quaternion.identity, parent.transform);
        chatBubble.GetComponent<ChatBubble>().Setup(text, 1f);

        // Remove after 'time' seconds
        Destroy(chatBubble, time);
    }
}
