using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogueTrigger : MonoBehaviour
{
    public DialogueGraph tree;

    bool alreadyPlayed = false;
    bool onePlayerIn = false;


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed && onePlayerIn) {
            TriggerDialogue();
            alreadyPlayed = true;
        }

        if (collision.gameObject.tag == "Player" && !onePlayerIn) {
            onePlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && onePlayerIn) {
            onePlayerIn = false;
        }
    }

    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(tree.nodes[0]);
    }

    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying) {
            SceneController.Instance.LoadScene("Main Menu");
        }
    }
}

