using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueTriggerCutsceneEnd : DialogueTrigger
{
    public Image cutsceneImage;

    bool imageShown = false;
    bool spacePressed = false;

    uint numOfPlayers = 0;
    [SerializeField] private int levelToLoad = 0;
    protected override void DoOnEnter(Collider collision) {
        if (collision.gameObject.tag == "Player") {
            numOfPlayers++;

            if (!alreadyPlayed && numOfPlayers == 2) {
                TriggerDialogue();
                alreadyPlayed = true;
            }
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            numOfPlayers--;
        }
    }


    //gonna be real this is probably a terrible way to do this
    //but like it works so
    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying && !imageShown) {
            cutsceneImage.DOFade(1, 0.5f);
            imageShown = true;
        }

        if (imageShown && Input.GetKeyDown(KeyCode.Space)) {
            cutsceneImage.DOFade(0, 0.5f);
            spacePressed = true;
        }

        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying && spacePressed) {
            SceneController.Instance.LoadScene(levelToLoad);
        }
    }
}
