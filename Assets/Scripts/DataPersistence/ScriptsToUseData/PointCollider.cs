using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PointCollider : MonoBehaviour, IDataPersistence
{
    public enum PointType { SHEEP, END}
    public PointType type;
    public int index;


    bool playerOnePassed = false;
    bool playerTwoPassed = false;


    private void Start() {
     GetComponent<BoxCollider>().isTrigger = true;
    }

    public void LoadData(GameData data)
    {
        return;
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerOnePassed && playerTwoPassed)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if(type == PointType.SHEEP) {
                DataPersistenceManager.Instance.SaveSheep(index);
                playerOnePassed = true;
                playerTwoPassed = true;
                Debug.Log("I am a free sheep");
                return;
            }
            else if (type == PointType.END) {
                bool? isOne = other.gameObject.GetComponent<Player>().isPlayerOne;

                if (isOne != null)
                {
                    if (isOne == true && !playerOnePassed) {
                        playerOnePassed = true;
                    }
                    if (isOne == false && !playerTwoPassed) {
                        playerTwoPassed = true;
                    }

                    if(playerOnePassed && playerTwoPassed) {
                        DataPersistenceManager.Instance.SaveLevel(index);
                    }
                }
            }
            
        }
    }
}
