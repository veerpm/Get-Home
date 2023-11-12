using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public AudioSource audioSource;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;

    public TextMeshProUGUI[] pauseButtons;
    public TextMeshProUGUI[] controlsButtons;
    public TextMeshProUGUI[] optionsButtons;

    private int pos; // position of cursor in menu
    private string cursor = "> ";
    private int menu;
    private float volume;
    private TextMeshProUGUI[][] menus;

    private bool activated;


    public void Start()
    {
        // setup the menus
        menus = new TextMeshProUGUI[][] { pauseButtons, controlsButtons, optionsButtons };

        // setup volume "bar"
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        optionsButtons[0].text = optionsButtons[0].text + (volume * 100).ToString("0") + "%";
    }

    public void Activate(bool activate)
    {
        activated = activate;

        // initialize cursor & pause menu

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);

        if (activate)
        {
            // add cursor
            pos = 0;
            menu = 0;
            menus[menu][pos].text = cursor + menus[menu][pos].text;
        }
        else
        {
            // remove cursor
            menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
        }
    }

    // player's interaction with cursor
    public void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveCursor(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                MoveCursor(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                ActivateButton(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                ActivateButton(-1);
            }
        }
    }

    public void displayOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        controlsMenu.SetActive(false);

        SetCursor(0, 2); // first button, options menu
    }

    public void displayPauseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
  
        SetCursor(0, 0); // first button, pause menu
    }

    public void displayControls()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true);

        SetCursor(0, 1); // first button, controls menu
    }

    // moves cursor up & down on active menu
    public void MoveCursor(int direction)
    {

        // move up cursor
        if (direction > 0 && pos > 0)
        {
            menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
            pos = pos - 1;
            menus[menu][pos].text = cursor + menus[menu][pos].text;
        }
        // move down cursor
        else if (direction < 0 && pos < menus[menu].Length - 1)
        {
            menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
            pos = pos + 1;
            menus[menu][pos].text = cursor + menus[menu][pos].text;
        }
    }

    public void SetCursor(int newPos, int newMenu)
    {
        menus[menu][pos].text = menus[menu][pos].text.Replace(cursor, "");
        pos = newPos;
        menu = newMenu;
        menus[menu][pos].text = cursor + menus[menu][pos].text;
    }

    // update player preference regarding volume & ingame volume
    public void MoveVolume(int direction)
    {
        string ancientVolume = (volume * 100).ToString("0") + "%";

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

        optionsButtons[0].text = optionsButtons[0].text.Replace(ancientVolume, newVolume);
        PlayerPrefs.SetFloat("Volume", volume);

        // actual change
        ModifyGeneralVolume(volume);
    }

    public void ActivateButton(int direction)
    {
        // activate button in 'main menu' at 'pos'. Activation must be to the right.
        if (menu == 0 && direction > 0)
        {
            switch (pos)
            {
                case 0: // start
                    displayControls();
                    break;
                case 1: // options
                    displayOptions();
                    break;
            }
        }

        // return button of controls
        else if (menu == 1)
        {
            if(direction == 1)
            {
                displayPauseMenu();

            }
        }

        // activate button in 'options' at 'pos'
        else if (menu == 2)
        {
            switch (pos)
            {
                case 0: // volume
                    MoveVolume(direction);
                    break;
                case 1: // quit
                    if (direction != 1)
                    {
                        // activation only allowed through right
                        return;
                    }
                    displayPauseMenu();
                    break;
            }
        }

    }

    private void ModifyGeneralVolume(float volume)
    {
        AudioSource[] sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        
        foreach (AudioSource audioSource in sources)
        {
            audioSource.volume = volume;
        }
    }
}
