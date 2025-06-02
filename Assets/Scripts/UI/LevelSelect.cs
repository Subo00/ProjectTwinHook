using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private uint numOfCompletedLevels = 0;
    [SerializeField] private GameObject buttonContainer;
    void Start() {
        numOfCompletedLevels = DataPersistenceManager.Instance.getData().numOfCompletedLevels;
        Button [] buttons = buttonContainer.GetComponentsInChildren<Button>();

        for(int i = 0; i < buttons.Length; i++) {
            if (i <= numOfCompletedLevels) {
                buttons[i].interactable = true;
            }
            else {
                buttons[i].interactable = false;
            }
        }
    }

   
}
