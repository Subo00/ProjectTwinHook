using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonController : MonoBehaviour, IMyUpdate {
    public MovingPlatform[] platforms;
    
    private MovingPlatform buttonPusher;
    private uint numOfStanders = 0;

    private void Start() {
        buttonPusher = GetComponentInChildren<MovingPlatform>();
    }

    void IMyUpdate.MyUpdate() {
        for(int i = 0; i < platforms.Length; i++) {
            platforms[i].SetBool(true);
        }
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
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].SetBool(false);
        }
    }
}
