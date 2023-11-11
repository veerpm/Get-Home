using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BarkGroup
{
    public GameObject[] enemies;
    public TextAsset barksFile;
}
public class ChatManager : MonoBehaviour
{
    public GameObject player;
    public GameObject chatObject;
    public List<BarkGroup> barkGroups;
    //public GameObject[] enemies;
    //public TextAsset barksFile;

    //private string[] barks;

    // Variable player and Start method are only there for TESTING purposes (calling the method Create()
    // is simpler if you want to create dialogues).

    public void Start()
    {
        //CreateBubble(player, "I need to get home!", 3f);
        //StartCoroutine(chatDemonstration());

        // For each group of enemies, cycle through barks
        foreach(BarkGroup group in barkGroups)
        {
            TextAsset barksFile = group.barksFile;
            GameObject[] enemies = group.enemies;

            if (barksFile != null && enemies != null)
            {
                string[] barks = barksFile.ToString().Split('\n');
                StartCoroutine(RandomEnemyChat(enemies, barks));
            }
        }

    }


    // Params = original, Vector3 position, Quaternion rotation, Transform parent
    public GameObject CreateBubble(GameObject parent, string text, float time, float textSize = 1f, Vector3 localPosition = default(Vector3))
    {
        // move slightly above head if no pos. given
        if (localPosition == default(Vector3))
        {
            localPosition = new Vector3(0, 1f, 0);
        }

        // create & set dialogue
        GameObject chatBubble = Instantiate(chatObject, parent.transform.position + localPosition,
            Quaternion.identity, parent.transform);
        // standardize size
        Vector3 parentScale = parent.transform.localScale;
        chatBubble.transform.localScale = new Vector3(3/ Mathf.Abs(parentScale.x), 3/parentScale.y, 1);
        chatBubble.GetComponent<ChatBubble>().Setup(text, textSize);

        // Remove after 'time' seconds
        if(time != 0)
        {
            Destroy(chatBubble, time);
        }
        return chatBubble;
    }

    IEnumerator RandomEnemyChat(GameObject[] enemies, string[] barks)
    {
        float time = 2.5f;

        // constantly print enemy dialogues
        while (true)
        {
            int randBark = Random.Range(0, barks.Length);
            GameObject randEnemy = enemies[Random.Range(0,enemies.Length)];

            // if enemy isn't dead, create speech bubble
            if (!randEnemy.GetComponent<EnemyHealth>().IsDead())
            {
                CreateBubble(randEnemy, barks[randBark], time);
            }

            yield return new WaitForSeconds(time);
        }
    }
}
