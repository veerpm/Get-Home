using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    public GameObject gameManager;
    public Image blackScreen;
    public float fadeSpeed = 0.001f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // change scene
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");
            blackScreen.enabled = true;
            StartCoroutine(FadeBlack());
        }
    }

    IEnumerator LoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator FadeBlack()
    {
        Color color;
        float fadeAmount;

        // fade to black
        while(blackScreen.color.a < 1)
        {
            color = blackScreen.color;
            fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

            blackScreen.color = new Color(color.r, color.g, color.b, fadeAmount);
            yield return null;
        }

        // if paused (either with cutscene or 'p'), wait for game to continue
        while (gameManager.GetComponent<GamePause>().isPaused())
        {
            yield return null;
        }


        // change scene once screen is black
        StartCoroutine(LoadScene());
    }
}
