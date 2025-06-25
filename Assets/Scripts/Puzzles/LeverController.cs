using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeverController : MonoBehaviour, IMyUpdate {
    public MovingPlatform[] platforms;

    private MovingPlatform leverHandle;
    private uint numOfStanders = 0;

    private bool isPlatformMoved = false;
    public bool setOneTime = false;
    private bool isOneTime = false;

    private void Start() {
        leverHandle = GetComponentInChildren<MovingPlatform>();
    }

    void IMyUpdate.MyUpdate() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].SetBool(true);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !isOneTime) {

            if (!isPlatformMoved) {
                MoveButtonDown();
            }
            else{
                MoveButtonUp();
            }
            isPlatformMoved = !isPlatformMoved;  
        }
    }

    //private void OnTriggerExit(Collider other) {

    //    if (other.gameObject.tag == "Player") {
    //        numOfStanders--;
    //        if (numOfStanders == 0) {
    //            MoveButtonUp();
    //        }
    //    }
    //}

    private void MoveButtonDown() {
        UpdateManager.Instance.AddUpdatable(this);

        leverHandle.SetBool(true);
        if(setOneTime)
        {
            isOneTime = true;
        }
    }

    private void MoveButtonUp() {
        UpdateManager.Instance.RemoveUpdatable(this);

        leverHandle.SetBool(false);
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].SetBool(false);
        }
    }
}
