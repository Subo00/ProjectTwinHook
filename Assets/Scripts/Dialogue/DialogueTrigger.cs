using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueTrigger : MonoBehaviour {
    public DialogueGraph tree;

    bool alreadyPlayed = false;


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            TriggerDialogue();
            alreadyPlayed = true;
        }
    }

    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(tree.nodes[0]);
    }
}
