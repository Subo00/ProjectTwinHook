using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] Vector3 endPos;

    [SerializeField] float speed;

    [SerializeField] bool isButtonPusher; 
    bool buttonActive = false;

    // Start is called before the first frame update
    void Start() {
        startPos = transform.position;
        if(isButtonPusher ) {
            endPos += startPos;
        }
    }

    // Update is called once per frame
    void Update() {
        if(buttonActive) {
            //go towards endPos
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        }
        else {
            //go towards startPos
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        }
    }

    public void SetBool(bool isActive) {
        buttonActive = isActive;
    }
}
