using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using DG.Tweening;

public class DialogueManager : MonoBehaviour {
    public TMP_Text dialogueText;
    public TMP_Text speakerName;
    public Image portrait;

    public Button nextButton;
    public CanvasGroup canvasGroup;

    public Vector3 showPanelPos = new Vector3(0, -1, 0);
    public Vector3 hidePanelPos = new Vector3(0, -400, 0);

    public float panelAnimationTime = 0.7f;
    public float textSpeed = 0.08f;

    public bool dialogueIsPlaying = false;

    Node currentNode;
    Queue<string> sentences;
    bool canContinueToNextLine;
    AudioSource source;
    AudioClip talkingClip;

    private void Start() {
        sentences = new Queue<string>();
        source = GetComponent<AudioSource>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    private void Update() {
        if (dialogueIsPlaying && canContinueToNextLine && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))) {
            nextButton.onClick.Invoke();
        }
    }

    public void StartDialogue(Node rootNode) {
        StopAllCoroutines();
        dialogueIsPlaying = true;
        currentNode = rootNode;

        canvasGroup.interactable = true;

        //check for types (either regular or end)
        if (currentNode.GetType() == typeof(DialogueEndNode)) {
            EndDialogue();
        }
        else {

            DialogueNode dialogueNode = currentNode as DialogueNode;
            Dialogue dialogue = dialogueNode.dialogue;

            //set panel
            speakerName.text = dialogue.name;
            talkingClip = dialogue.talkingClip;
            portrait.sprite = dialogue.portrait;

            //set buttons
            nextButton.gameObject.SetActive(true);


            sentences.Clear();
            for (int i = 0; i < dialogue.dialogue.Length; i++) {
                sentences.Enqueue(dialogueNode.dialogue.dialogue[i]);
            }

            //bring up the panel
            transform.DOLocalMove(showPanelPos, panelAnimationTime).OnComplete(() => DisplaySentence());


        }
    }

    public void DisplaySentence() {
        StopAllCoroutines();
        StartCoroutine(RenderSentence(sentences.Dequeue()));
    }

    IEnumerator RenderSentence(string sentence) {
        dialogueText.text = "";

        canContinueToNextLine = false;
        nextButton.gameObject.SetActive(false);

        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i < sentence.Length; i++) {

            if (Input.GetKeyDown(KeyCode.Space)) { //allow for multiple inputs?
                dialogueText.maxVisibleCharacters = sentence.Length;
                break;
            }

            dialogueText.maxVisibleCharacters++;

            if (i % 4 == 0) {
                source.PlayOneShot(talkingClip);
            }
            yield return new WaitForSeconds(textSpeed);
        }

        canContinueToNextLine = true;
        nextButton.gameObject.SetActive(true);
    }

    public void DisplayNext() {
        DialogueNode dialogueNode = currentNode as DialogueNode;
        NodePort port = dialogueNode.GetOutputPort("nextNode").Connection;

        if (port != null) {
            currentNode = port.node;
        }

        StartDialogue(currentNode);
    }

    public void EndDialogue() {
        canvasGroup.interactable = false;
        nextButton.gameObject.SetActive(false); //ensure that this doesn't get pressed by accident
        StopAllCoroutines();
        dialogueText.text = ""; //ensure that the old text doesn't show when the panel moves back up
        transform.DOLocalMove(hidePanelPos, panelAnimationTime).OnComplete(() => { dialogueIsPlaying = false; });
    }
}
