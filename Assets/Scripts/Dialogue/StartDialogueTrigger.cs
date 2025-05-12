using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueTrigger : MonoBehaviour
{
    public DialogueGraph tree;
    public GameObject platform;

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

    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying) {
            platform.SetActive(false);
        }
    }
}
