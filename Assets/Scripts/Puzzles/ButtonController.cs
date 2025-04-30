using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public GameObject door;
    public int raiseDoor = 5;
    public Transform buttonPusher;
    public Vector3 doorClosedPos;
    public Vector3 doorOpenedPos;

    bool doorOpen = false;

    float holdTimer = 0;
    bool onButton = false;

    Vector3 moveTo;
    Vector3 moveButton;
    

    private void Start() {
        //set the vectors that will be used to move the door and the button up and down
        moveTo = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        moveButton = new Vector3(0, -0.2f, 0);
        doorClosedPos = door.transform.position;
        doorOpenedPos = door.transform.position;
        doorOpenedPos.y += 5f;
    }

    private void Update() {

        //if we're standing on the button and the door isn't open, open it after a short delay
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
        doorOpen = true;
        moveTo = doorOpenedPos;
        door.transform.DOLocalMove(moveTo, 0.5f);
    }

    private void CloseDoor() {
        moveTo = doorClosedPos;
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
