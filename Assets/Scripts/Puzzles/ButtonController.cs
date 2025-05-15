using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonController : MonoBehaviour, IMyUpdate {
    public MovingPlatform platform;
    
    private MovingPlatform buttonPusher;
    private uint numOfStanders = 0;

    private void Start() {
        buttonPusher = GetComponentInChildren<MovingPlatform>();
    }

    void IMyUpdate.MyUpdate() {
        platform.SetBool(true);
    }
   

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            numOfStanders++;
            if(numOfStanders == 1) {
                MoveButtonDown();
            }
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Player") {
            numOfStanders--;
            if (numOfStanders == 0) {
                MoveButtonUp();
            }
        }
    }

    private void MoveButtonDown() {
        UpdateManager.Instance.AddUpdatable(this);
        buttonPusher.SetBool(true);
    }

    private void MoveButtonUp() {
        UpdateManager.Instance.RemoveUpdatable(this);
        buttonPusher.SetBool(false);
        platform.SetBool(false);
    }
}
