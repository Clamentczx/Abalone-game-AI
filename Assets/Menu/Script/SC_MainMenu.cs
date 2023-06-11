using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu: MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject ChooseMenu;


    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Normal");
    }

    public void againstAI()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void watch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Advance");
    }

    //credit menu
    public void CreditsButton()
    {   
       
       ChooseMenu.SetActive(false);
       MainMenu.SetActive(false);
       CreditsMenu.SetActive(true);

    }

    //main menu
    public void MainMenuButton()
    {
        ChooseMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        MainMenu.SetActive(true);
     
    }
    
    //chose modes menu
    public void ChooseMenuButton()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ChooseMenu.SetActive(true);
    }
    
    //exit game 
    public void QuitButton()
    {
        Application.Quit();
    }
}
