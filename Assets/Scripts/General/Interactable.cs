using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IMyUpdate
{
    protected bool inUse = false;
    protected UIManager uiManager;
    protected Transform dropPoint = null;
    protected bool isPlayerOneNear = false;
    protected bool isPlayerTwoNear = false;
    protected abstract void OnUpdate();
    void IMyUpdate.MyUpdate(){
        OnUpdate();
    }
    protected void CommonLogic(){
        if (inUse){
            //add statments...
            //TODO: think about can an object be interacted with both players
            //if not, then just remove prompts,
            //if so.... remove prompts from the player that is using it...
        }
        else{
            if (isPlayerOneNear){
                uiManager.camOne.ShowInteractionOnObject(dropPoint);
            }
            if (isPlayerTwoNear){
                uiManager.camTwo.ShowInteractionOnObject(dropPoint);
            }
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
        if (other.CompareTag("Player")){
            bool isOne = other.gameObject.GetComponent<Player>().isPlayerOne;
            SetBool(isOne, true);

            //if both players are near, no need to add to updateable twice
            if (isPlayerOneNear ^ isPlayerTwoNear){ 
                UpdateManager.Instance.AddUpdatable(this);
            }
            //uiManager.SetInteractPoint(dropPoint);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            //uiManager.SetInteractPoint();
            bool isOne = other.gameObject.GetComponent<Player>().isPlayerOne;
            SetBool(isOne, false);

            if (!(isPlayerOneNear || isPlayerTwoNear)){
                UpdateManager.Instance.RemoveUpdatable(this);
            }
            uiManager.HideInteraction(); //hide both interactions, because other cam will 
                                         //update interaction on object
        }
    }

    private void OnDisable()  {
        UpdateManager.Instance.RemoveUpdatable(this);
        if(uiManager != null) uiManager.HideInteraction();
    }

    private void SetBool(bool isPlayerOne, bool value){
        if (isPlayerOne){
            isPlayerOneNear = value;
        }
        else{
            isPlayerTwoNear = value;
        }
    }
}