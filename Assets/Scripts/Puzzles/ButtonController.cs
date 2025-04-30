using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public GameObject door;
    public int raiseDoor = 5;
    public Transform buttonPusher;

    bool doorOpen = false;

    float holdTimer = 0;
    bool onButton = false;

    Vector3 moveTo;
    Vector3 moveButton;
    

    private void Start() {
        moveTo = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        moveButton = new Vector3(0, -0.2f, 0);
    }

    private void Update() {

        if (onButton && !doorOpen) {
            holdTimer += Time.deltaTime;
        }

        if (holdTimer > 0.5 && !doorOpen) {
            OpenDoor();
            holdTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !doorOpen) {
            onButton = true;
            //OpenDoor();

            MoveButtonDown();
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Player") {
            onButton = false;
            MoveButtonUp();
        }
            
        if (other.gameObject.tag == "Player" && doorOpen) {
            CloseDoor();
        }
    }

    private void OpenDoor() {
        print("open door");
        //transform.DOLocalMove(hidePanelPos, panelAnimationTime).OnComplete(() => { dialogueIsPlaying = false; });
        //moveTo = new Vector3(door.transform.position.x,door.transform.position.y+raiseDoor,door.transform.position.z); //there's probably something something efficiency
        moveTo.y = door.transform.position.y + raiseDoor;
        door.transform.DOLocalMove(moveTo, 0.5f);
        doorOpen = true;
    }

    private void CloseDoor() {
        moveTo.y = door.transform.position.y - raiseDoor;
        door.transform.DOLocalMove(moveTo, 0.5f);
        doorOpen = false;
    }

    private void MoveButtonDown() {
        moveButton.y = 0;
        buttonPusher.DOLocalMove(moveButton, 0.2f);
    }

    private void MoveButtonUp() {
        moveButton.y = 0.3f;
        buttonPusher.DOLocalMove(moveButton, 0.2f);
    }
}
