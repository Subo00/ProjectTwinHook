using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerEnd : DialogueTrigger {
    
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


    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying) {
            SceneController.Instance.LoadScene(levelToLoad);
        }
    }
}

