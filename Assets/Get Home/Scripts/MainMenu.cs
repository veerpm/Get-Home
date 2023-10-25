using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public TextMeshProUGUI[] mainButtons;
    public TextMeshProUGUI[] optionsButton;

    private int pos; // position of cursor in menu
    private string cursor = "> ";
    private int menu;
    private float volume;
    private TextMeshProUGUI[][] menus;

    public void Start()
    {
        // setup the menus
        menus = new TextMeshProUGUI[][] { mainButtons, optionsButton};

        // setup volume "bar"
        volume = 0f;
        optionsButton[0].text = optionsButton[0].text + "0%";

        // initialize cursor & main menu
        pos = 0;
        menu = 0;
        mainMenu.SetActive(true);
        mainButtons[pos].text = cursor + mainButtons[pos].text;
    }

    // player's interaction with cursor
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveCursor(1);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveCursor(-1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ActivateButton(1);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ActivateButton(-1);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void displayOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        menu = 1; // options
        SetCursor(0);
    }

    public void displayMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        menu = 0; // main menu
        SetCursor(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // moves cursor up & down on active menu
    public void MoveCursor(int direction)
    {

        // move up cursor
        if(direction > 0 && pos > 0)
        {
            menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
            pos = pos - 1;
            menus[menu][pos].text = cursor + menus[menu][pos].text;
        }
        // move down cursor
        else if (direction < 0 && pos < menus[menu].Length-1)
        {
            menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
            pos = pos + 1;
            menus[menu][pos].text = cursor + menus[menu][pos].text;
        }
    }

    public void SetCursor(int newPos)
    {
        menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
        pos = newPos;
        menus[menu][pos].text = cursor + menus[menu][pos].text;
    }

    public void MoveVolume(int direction)
    {
        string ancientVolume = (volume*100).ToString("0") + "%";

        if (direction > 0 && volume < 0.99)
        {
            volume += 0.1f;
        }
        else if (direction < 0 && volume > 0.01)
        {
            volume -= 0.1f;
        }
        Debug.Log(volume);
        string newVolume = (volume * 100).ToString("0") + "%";

        optionsButton[0].text = optionsButton[0].text.Replace(ancientVolume, newVolume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void ActivateButton(int direction)
    {
        // activate button in 'main menu' at 'pos'. Activation must be to the right.
        if(menu == 0 && direction > 0)
        {
            switch (pos)
            {
                case 0: // start
                    PlayGame();
                    break;
                case 1: // options
                    displayOptions();
                    break;
                case 2: // quit
                    QuitGame();
                    break;
            }
        }
        // activate button in 'options' at 'pos'
        else if(menu == 1)
        {
            switch (pos)
            {
                case 0: // volume
                    MoveVolume(direction);
                    break;
                case 1: // quit
                    if(direction < 0)
                    {
                        // activation only allowed through right
                        return;
                    }
                    displayMainMenu();
                    break;
            }
        }

    }
}
