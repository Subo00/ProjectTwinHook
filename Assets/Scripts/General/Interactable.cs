using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IMyUpdate
{
    protected bool inUse = false;
    protected UIManager uiManager;
    protected Transform dropPoint = null;

    protected abstract void OnUpdate();
    void IMyUpdate.MyUpdate(){
        OnUpdate();
    }
    protected void CommonLogic(){
        if (inUse){
            uiManager.HideInteraction();
        }
        else{
            uiManager.ShowInteractionOnObject(dropPoint);
        }
    }

    protected virtual void Start(){
        //Make sure that the gameObject dropPoint is a child of the GO
        //that this script is attached to
        Transform[] temp = gameObject.GetComponentsInChildren<Transform>();
        dropPoint = temp[1];
        if (dropPoint == null){
            Debug.LogError("dropPoint game object can not be found!");
        }
        uiManager = UIManager.Instance;
    }

    //Make sure that the GO this script is attached to has a Collider
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("PlayerInteraction")){
            UpdateManager.Instance.AddUpdatable(this);
            inUse = false;
            uiManager.SetInteractPoint(dropPoint);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("PlayerInteraction")){
            uiManager.SetInteractPoint();
            uiManager.HideInteraction();
            UpdateManager.Instance.RemoveUpdatable(this);
        }
    }
}