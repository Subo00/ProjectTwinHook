using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogueTrigger : MonoBehaviour
{
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

    private void Update() {
        if (alreadyPlayed && !dialogueManager.dialogueIsPlaying) {
            SceneController.Instance.LoadScene("Main Menu");
        }
    }
}

