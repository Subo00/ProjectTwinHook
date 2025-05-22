using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinHookController;
public class DialogueTriggerPlayerDependet : DialogueTrigger
{
    public DialogueGraph treePlayerTwo;
    protected override void DoOnEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            //it has to have a player script if it has a player tag
            if (collision.GetComponent<Player>().isPlayerOne) {
                DialogueManager.Instance.StartDialogue(tree.nodes[0]);
            } else {
                DialogueManager.Instance.StartDialogue(treePlayerTwo.nodes[0]);
            }

            alreadyPlayed = true;
        }
    }
}
