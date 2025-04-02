using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI<MenuUI>
{
    private Button resumeButton;
    private Button startMenuButton;
    private Button optionsButton;
    private Button quitButton;

    private void Start()
    {
        //make sure the buttons align in order in editor
        Button[] buttons = GetComponentsInChildren<Button>();
        resumeButton = buttons[0];
        startMenuButton = buttons[1];
        optionsButton = buttons[2];
        quitButton = buttons[3];

        resumeButton.onClick.AddListener(UIManager.Instance.ToggleMenu);
        startMenuButton.onClick.AddListener(SceneController.Instance.LoadStartMenu);
        startMenuButton.onClick.AddListener(() => { Time.timeScale = 1.0f; });
        quitButton.onClick.AddListener(SceneController.Instance.QuitGame);
    }

}