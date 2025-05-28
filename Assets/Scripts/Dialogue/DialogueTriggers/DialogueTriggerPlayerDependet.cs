using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinHookController;
public class DialogueTriggerPlayerDependet : DialogueTrigger
{
    public DialogueGraph treePlayerTwo;
    protected override void DoOnEnter(Collider collision) {

        Debug.Log("I AM HERE");
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            Debug.Log("I AM PLAYER");

            //it has to have a player script if it has a player tag
            if (collision.GetComponent<Player>().isPlayerOne == true) {
                DialogueManager.Instance.StartDialogue(tree.nodes[0]);
                Debug.Log("I AM PLAYER ONE");

            }
            else {
                DialogueManager.Instance.StartDialogue(treePlayerTwo.nodes[0]);
                Debug.Log("I AM PLAYER TWO");
                
            }

            alreadyPlayed = true;
        }
    }
}
