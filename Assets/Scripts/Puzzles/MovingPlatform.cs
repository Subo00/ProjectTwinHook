using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // In MovingPlatform.cs
    protected Vector3 startPos;
    [SerializeField] protected Vector3 endPos;

    [SerializeField] protected float speed = 10;

    [SerializeField] protected bool isButtonPusher;
    [SerializeField] protected bool isLever;

    protected Quaternion startRot;
    [SerializeField] protected Quaternion endRot;

    protected bool buttonActive = false;

    // Make SetBool overridable
    public virtual void SetBool(bool isActive)
    {
        buttonActive = isActive;
    }


    // Start is called before the first frame update
    void Start() {
        startPos = transform.position;
        if(isButtonPusher || isLever) {
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

}
