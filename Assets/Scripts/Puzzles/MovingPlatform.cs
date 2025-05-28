using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    protected Vector3 startPos;
    [SerializeField] protected Vector3 endPos;

    [SerializeField] float speed = 10;

    [SerializeField] bool isLever;
    Quaternion startRot;
    [SerializeField] Quaternion endRot;

    bool buttonActive = false;

    // Start is called before the first frame update
    protected virtual void Start() {
        startPos = transform.position;
        if(isLever) {
            endPos += startPos;
        }

        if (isLever) {
            startRot = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update() {
        if(buttonActive) {

            

            if (isLever) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, speed * Time.deltaTime);
            }
            else {
                //go towards endPos
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            }

        }
        else {
            

            if (isLever) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot, speed * Time.deltaTime);
            }
            else {
                //go towards startPos
                transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            }
   
        }
    }

    public void SetBool(bool isActive) {
        buttonActive = isActive;
    }
}
