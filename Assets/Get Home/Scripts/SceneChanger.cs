using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // change scene
        if (other.gameObject.tag == "Player")
            StartCoroutine(LoadScene());

    }

    IEnumerator LoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
