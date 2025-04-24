using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI<MenuUI>
{
    private Button resumeButton;
    private Button startMenuButton;
   // private Button optionsButton;
    private Button saveButton;
    private Button loadButton;
    private Button quitButton;

    private void Start(){
        //make sure the buttons align in order in editor
        Button[] buttons = GetComponentsInChildren<Button>();
        resumeButton = buttons[0];
        startMenuButton = buttons[1];
        //optionsButton = buttons[2];
        saveButton = buttons[2];
        loadButton = buttons[3];
        quitButton = buttons[4];

        resumeButton.onClick.AddListener(UIManager.Instance.ToggleMenu);
        saveButton.onClick.AddListener(DataPersistenceManager.Instance.SaveGame);
        loadButton.onClick.AddListener(UIManager.Instance.ToggleMenu); 
        //loadButton.onClick.AddListener(() => { Time.timeScale = 1.0f; });
        loadButton.onClick.AddListener(DataPersistenceManager.Instance.LoadGame);

        // loadButton.onClick.AddListener(SceneController.Instance.LoadStartMenu);
        startMenuButton.onClick.AddListener(SceneController.Instance.LoadStartMenu);
        startMenuButton.onClick.AddListener(() => { Time.timeScale = 1.0f; });
        
        quitButton.onClick.AddListener(SceneController.Instance.QuitGame);
    }

}