using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueTrigger : MonoBehaviour {
    public DialogueGraph tree;
    public DialogueManager dialogueManager;

    bool alreadyPlayed = false;


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            TriggerDialogue();
            alreadyPlayed = true;
        }
    }

    public void TriggerDialogue() {
        dialogueManager.StartDialogue(tree.nodes[0]);
    }
}
