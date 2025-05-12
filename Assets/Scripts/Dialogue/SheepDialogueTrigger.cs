using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepDialogueTrigger : MonoBehaviour
{
    public DialogueGraph tree;
    public GameObject sheep;

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
            sheep.SetActive(false);
            //here would also be code to increment the player's sheep count and such
        }
    }
}
