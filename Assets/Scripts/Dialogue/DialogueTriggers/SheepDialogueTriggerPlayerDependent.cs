using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;

public class SheepDialogueTriggerPlayerDependent : MonoBehaviour
{
    public DialogueGraph tree;
    public GameObject sheep;

    public DialogueGraph treePlayerTwo;

    bool alreadyPlayed = false;


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {

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


    private void Update() {
        if (alreadyPlayed && !DialogueManager.Instance.dialogueIsPlaying) {
            sheep.SetActive(false);
            //here would also be code to increment the player's sheep count and such
        }
    }
}
