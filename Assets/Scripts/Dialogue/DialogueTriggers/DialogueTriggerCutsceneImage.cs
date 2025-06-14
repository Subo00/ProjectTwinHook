using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueTriggerCutsceneImage : DialogueTrigger
{
    public GameObject platform;
    public AudioSource musicController;
    public Image cutsceneImage;

    bool imageShown = false;
    bool spacePressed = false;

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
            platform.SetActive(false);
            musicController.Play();
            Destroy(this.gameObject); //we don't need it anymore 
        }
    }
}
