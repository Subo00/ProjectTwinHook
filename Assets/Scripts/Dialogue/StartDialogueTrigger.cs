using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDialogueTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public DialogueGraph tree;
    public DialogueManager dialogueManager;
    public Image fader;
    public Image cursor;

    bool alreadyPlayed = false;

    private void Update() {
        if (alreadyPlayed && !dialogueManager.dialogueIsPlaying) {
            fader.DOFade(0, 4).OnComplete(() => { 
                audioSource.Play();
                cursor.gameObject.SetActive(true); 
                this.gameObject.SetActive(false); 
            });
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player" && !alreadyPlayed) {
            TriggerDialogue();
            cursor.gameObject.SetActive(false);
            alreadyPlayed = true;
        }
    }

    public void TriggerDialogue() {
        dialogueManager.StartDialogue(tree.nodes[0]);
    }
}
