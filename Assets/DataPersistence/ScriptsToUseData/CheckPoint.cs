using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinHookController;

[RequireComponent(typeof(SphereCollider))]
public class CheckPoint : MonoBehaviour, IDataPersistence
{
    bool playerOnePassed = false;
    bool playerTwoPassed = false;

    void Start() {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void IDataPersistence.LoadData(GameData data) {
        //throw new System.NotImplementedException();
        return;
    }

    private void OnTriggerEnter(Collider other) {
        if (playerOnePassed && playerTwoPassed) {
            return;
        }
        if (other.CompareTag("Player")) {
            bool? isOne = other.gameObject.GetComponent<Player>().isPlayerOne;

            if(isOne != null){
                if(isOne == true && !playerOnePassed) {
                    playerOnePassed = true; Debug.Log("WU");
                    DataPersistenceManager.Instance.SaveCheckPoint(this.transform.position, true);
                }
                if(isOne == false && !playerTwoPassed) {
                    playerTwoPassed = true; Debug.Log("WO");
                    DataPersistenceManager.Instance.SaveCheckPoint(this.transform.position, false);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        /*
        if (isSaved) { return;  }
        data.playerPosition = this.transform.position;
        isSaved = true;
        //data.playerRotation = this.transform.rotation;
        */
        return;
    }
}
