using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinHookController;

public class CheckPoint : MonoBehaviour, IDataPersistence
{
    bool playerOnePassed = false;
    bool playerTwoPassed = false;
    bool isSaved = false;

    void IDataPersistence.LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSaved){
            return;
        }
        if (other.CompareTag("Player")){
            bool? isOne = other.gameObject.GetComponent<Player>().isPlayerOne;

            if(isOne != null){
                if(isOne == true) { playerOnePassed = true; Debug.Log("WU");  }
                else { playerTwoPassed = true; Debug.Log("WO"); }

                if(playerOnePassed && playerTwoPassed){
                    DataPersistenceManager.Instance.SaveGame();
                    isSaved = true;
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
        isSaved = true;
        //data.playerRotation = this.transform.rotation;
    }
}
