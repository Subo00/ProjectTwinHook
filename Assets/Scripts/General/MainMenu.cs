using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartGame(){
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    
   }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Menu() {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame ()
    {
        Application.Quit();
    }
}
