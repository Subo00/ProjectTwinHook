using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUITrigger : MonoBehaviour
{
    public Canvas movementPrompts;

    private void Start() {
        movementPrompts.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        //I'd like to get this cleaned up better
        //so that the prompts only appear once the dialogue is finished (read my dialogue)
        //and they only disappear once both players have left the area, not just one
        //That won't take long but it's low priority so I'm just going to leave it as is for now
        //and come back and polish it up later

        if (other.gameObject.tag == "Player") {
            movementPrompts.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            movementPrompts.enabled = false;
        }
    }
}
