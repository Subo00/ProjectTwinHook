using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[RequireComponent(typeof(BoxCollider))]
public class DialogueTrigger : MonoBehaviour {
    public DialogueGraph tree;

    protected bool alreadyPlayed = false;


    protected void OnTriggerEnter(Collider collision) {
        DoOnEnter(collision);
    }

    protected virtual void DoOnEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            TriggerDialogue();
            alreadyPlayed = true;
        }
    }

    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(tree.nodes[0]);
    }
}
