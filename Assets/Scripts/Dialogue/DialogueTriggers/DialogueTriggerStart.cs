using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerStart : DialogueTrigger
{
    public GameObject platform;
    public AudioSource musicController;

    //not effective way of checking things but okay 
    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying) {
            platform.SetActive(false);
            musicController.Play();
            Destroy(this.gameObject); //we don't need it anymore 
        }
    }
}
