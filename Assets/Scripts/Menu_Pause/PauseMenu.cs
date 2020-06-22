using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModuloKart.Controls;

public enum PauseMenuOption
{
    resume,
    menu,
    options,
    quit,
}

public enum OptionsMenuOption
{
    volume,
    fullscreen,
    motionblur,
}

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public static bool GameIsInOptions = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    public GameObject ResumeButton;
    public GameObject MenuButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;

    public GameObject VolumeSlider;
    public GameObject FullscreenToggle;
    public GameObject MotionblurToggle;

    public PauseMenuOption mainMenuOption;
    public OptionsMenuOption optionsMenuOption;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    private void Update()
    {
        PauseMenuNext();
        PauseMenuPrev();
        ConfirmPauseMenuOption();

        //OptionsMenuNext();
        //OptionsMenuPrev();
        //ConfirmOptionsMenuOption();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("StartButton_ANYPLAYER"))
        {
            if (GameIsPaused && !GameIsInOptions)
            {
                Resume();
            }
            else if (GameIsPaused && GameIsInOptions)
            {
                ShowPause();
                GameIsInOptions = false;
            }          
            else
            {
                Pause();
            }

            GetMenuOptions();
            InitPauseMenu();

        }
    }


    bool isPressPrev;
    bool isPressPrevRelease;
    private void PauseMenuPrev()
    {
        if (GameIsPaused)
        {
            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") < 0)
            {
                isPressPrev = true;
            }
            if (isPressPrev)
            {
                if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
                {
                    isPressPrev = false;
                    isPressPrevRelease = true;
                }
            }
            if (isPressPrevRelease)
            {
                isPressPrevRelease = false;

                switch (mainMenuOption)
                {
                    case PauseMenuOption.resume:
                        mainMenuOption = PauseMenuOption.quit;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(true);
                        break;
                    case PauseMenuOption.menu:
                        mainMenuOption = PauseMenuOption.resume;
                        ResumeButton.SetActive(true);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(false);
                        break;
                    case PauseMenuOption.options:
                        mainMenuOption = PauseMenuOption.menu;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(true);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(false);
                        break;
                    case PauseMenuOption.quit:
                        mainMenuOption = PauseMenuOption.options;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(true);
                        QuitButton.SetActive(false);
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }
    }


    bool isPressNext;
    bool isPressNextRelease;
    private void PauseMenuNext()
    {
        if (GameIsPaused)
        {
            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") > 0)
            {
                isPressNext = true;
            }
            if (isPressNext)
            {
                if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
                {
                    isPressNext = false;
                    isPressNextRelease = true;
                }
            }
            if (isPressNextRelease)
            {
                isPressNextRelease = false;

                switch (mainMenuOption)
                {
                    case PauseMenuOption.resume:
                        mainMenuOption = PauseMenuOption.menu;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(true);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(false);
                        break;
                    case PauseMenuOption.menu:
                        mainMenuOption = PauseMenuOption.options;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(true);
                        QuitButton.SetActive(false);
                        break;
                    case PauseMenuOption.options:
                        mainMenuOption = PauseMenuOption.quit;
                        ResumeButton.SetActive(false);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(true);
                        break;
                    case PauseMenuOption.quit:
                        mainMenuOption = PauseMenuOption.resume;
                        ResumeButton.SetActive(true);
                        MenuButton.SetActive(false);
                        OptionsButton.SetActive(false);
                        QuitButton.SetActive(false);
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }
    }

    private void InitPauseMenu()
    {
        mainMenuOption = PauseMenuOption.resume;
        ResumeButton.SetActive(true);
        MenuButton.SetActive(false);
        OptionsButton.SetActive(false);
        QuitButton.SetActive(false);
    }

    private void GetMenuOptions()
    {
        MenuSelectionOption[] temp = GameObject.FindObjectsOfType<MenuSelectionOption>();

        foreach (MenuSelectionOption t in temp)
        {
            if (t.mainMenuOption.Equals(PauseMenuOption.resume))
            {
                ResumeButton = t.bg;
            }
            else if (t.mainMenuOption.Equals(PauseMenuOption.menu))
            {
                MenuButton = t.bg;
            }
            else if (t.mainMenuOption.Equals(PauseMenuOption.options))
            {
                OptionsButton = t.bg;
            }
            else if (t.mainMenuOption.Equals(PauseMenuOption.quit))
            {
                QuitButton = t.bg;
            }
        }
    }

    private void ConfirmPauseMenuOption()
    {
        if (Input.GetButtonDown("A_ANYPLAYER"))
        {
            Debug.Log("Do Something");
            switch (mainMenuOption)
            {
                case PauseMenuOption.resume:
                    Resume();
                    break;
                case PauseMenuOption.menu:
                    Menu();
                    break;
                case PauseMenuOption.options:
                    ShowOptions();
                    break;
                case PauseMenuOption.quit:
                    Quit();
                    break;
                default:
                    Debug.Log("Unexpected Player Number Selection Option");
                    break;
            }
        }
    }

    //private void OptionsMenuPrev()
    //{
    //    if (GameIsPaused)
    //    {
    //        if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") < 0)
    //        {
    //            isPressPrev = true;
    //        }
    //        if (isPressPrev)
    //        {
    //            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
    //            {
    //                isPressPrev = false;
    //                isPressPrevRelease = true;
    //            }
    //        }
    //        if (isPressPrevRelease)
    //        {
    //            isPressPrevRelease = false;

    //            switch (optionsMenuOption)
    //            {
    //                case OptionsMenuOption.volume:
    //                    optionsMenuOption = OptionsMenuOption.motionblur;
    //                    MotionblurToggle.SetActive(true);
    //                    FullscreenToggle.SetActive(false);
    //                    break;
    //                case OptionsMenuOption.menu:
    //                    mainMenuOption = OptionsMenuOption.resume;
    //                    ResumeButton.SetActive(true);
    //                    MenuButton.SetActive(false);
    //                    OptionsButton.SetActive(false);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                case OptionsMenuOption.options:
    //                    mainMenuOption = OptionsMenuOption.menu;
    //                    ResumeButton.SetActive(false);
    //                    MenuButton.SetActive(true);
    //                    OptionsButton.SetActive(false);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                case OptionsMenuOption.quit:
    //                    mainMenuOption = OptionsMenuOption.options;
    //                    ResumeButton.SetActive(false);
    //                    MenuButton.SetActive(false);
    //                    OptionsButton.SetActive(true);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                default:
    //                    Debug.Log("Unexpected Player Number Selection Option");
    //                    break;
    //            }
    //        }
    //    }
    //}

    //private void OptionsMenuNext()
    //{
    //    if (GameIsPaused)
    //    {
    //        if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") > 0)
    //        {
    //            isPressNext = true;
    //        }
    //        if (isPressNext)
    //        {
    //            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
    //            {
    //                isPressNext = false;
    //                isPressNextRelease = true;
    //            }
    //        }
    //        if (isPressNextRelease)
    //        {
    //            isPressNextRelease = false;

    //            switch (mainMenuOption)
    //            {
    //                case PauseMenuOption.resume:
    //                    mainMenuOption = PauseMenuOption.menu;
    //                    ResumeButton.SetActive(false);
    //                    MenuButton.SetActive(true);
    //                    OptionsButton.SetActive(false);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                case PauseMenuOption.menu:
    //                    mainMenuOption = PauseMenuOption.options;
    //                    ResumeButton.SetActive(false);
    //                    MenuButton.SetActive(false);
    //                    OptionsButton.SetActive(true);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                case PauseMenuOption.options:
    //                    mainMenuOption = PauseMenuOption.quit;
    //                    ResumeButton.SetActive(false);
    //                    MenuButton.SetActive(false);
    //                    OptionsButton.SetActive(false);
    //                    QuitButton.SetActive(true);
    //                    break;
    //                case PauseMenuOption.quit:
    //                    mainMenuOption = PauseMenuOption.resume;
    //                    ResumeButton.SetActive(true);
    //                    MenuButton.SetActive(false);
    //                    OptionsButton.SetActive(false);
    //                    QuitButton.SetActive(false);
    //                    break;
    //                default:
    //                    Debug.Log("Unexpected Player Number Selection Option");
    //                    break;
    //            }
    //        }
    //    }
    //}

    //private void InitOptionsMenu()
    //{
    //    mainMenuOption = PauseMenuOption.resume;
    //    ResumeButton.SetActive(true);
    //    MenuButton.SetActive(false);
    //    OptionsButton.SetActive(false);
    //    QuitButton.SetActive(false);
    //}

    //private void GetOptionsMenuOptions()
    //{
    //    MenuSelectionOption[] temp = GameObject.FindObjectsOfType<MenuSelectionOption>();

    //    foreach (MenuSelectionOption t in temp)
    //    {
    //        if (t.mainMenuOption.Equals(PauseMenuOption.resume))
    //        {
    //            ResumeButton = t.bg;
    //        }
    //        else if (t.mainMenuOption.Equals(PauseMenuOption.menu))
    //        {
    //            MenuButton = t.bg;
    //        }
    //        else if (t.mainMenuOption.Equals(PauseMenuOption.options))
    //        {
    //            OptionsButton = t.bg;
    //        }
    //        else if (t.mainMenuOption.Equals(PauseMenuOption.quit))
    //        {
    //            QuitButton = t.bg;
    //        }
    //    }
    //}

    //private void ConfirmOptionsMenuOption()
    //{
    //    if (Input.GetButtonDown("A_ANYPLAYER"))
    //    {
    //        Debug.Log("Do Something");
    //        switch (mainMenuOption)
    //        {
    //            case PauseMenuOption.resume:
    //                Resume();
    //                break;
    //            case PauseMenuOption.menu:
    //                Menu();
    //                break;
    //            case PauseMenuOption.options:
    //                ShowOptions();
    //                break;
    //            case PauseMenuOption.quit:
    //                Quit();
    //                break;
    //            default:
    //                Debug.Log("Unexpected Player Number Selection Option");
    //                break;
    //        }
    //    }
    //}


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("should resume wooooooooo");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Menu works?");
    }

    public void ShowOptions()
    {
        Debug.Log("options work?");
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        GameIsInOptions = true;
       
    }

    public void ShowPause()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);

    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("should quit i hope it works");
    }

}