using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject chatObject;
    public GameObject[] enemies;

    // Variable player and Start method are only there for TESTING purposes (calling the method Create()
    // is simpler if you want to create dialogues).
    public GameObject player;

    public void Start()
    {
        //CreateBubble(player, "I need to get home!", 3f);
        //StartCoroutine(chatDemonstration());
    }


    // Params = original, Vector3 position, Quaternion rotation, Transform parent
    public void CreateBubble(GameObject parent, string text, float time, float textSize = 1f, Vector3 localPosition = default(Vector3))
    {
        // move slightly above head if no pos. given
        if (localPosition == default(Vector3))
        {
            localPosition = new Vector3(0, 1f, 0);
        }

        // create & set dialogue
        GameObject chatBubble = Instantiate(chatObject, parent.transform.position + localPosition,
            Quaternion.identity, parent.transform);
        chatBubble.GetComponent<ChatBubble>().Setup(text, textSize);

        // Remove after 'time' seconds
        Destroy(chatBubble, time);
    }

    IEnumerator chatDemonstration()
    {
        yield return new WaitForSeconds(4);
        CreateBubble(player, "This street is really weird!", 3f);
        foreach (GameObject enemy in enemies)
        {
            CreateBubble(enemy, "Get him!", 3f, 3f);
        }
    }
}
