using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isOne = other.gameObject.GetComponent<Player>().isPlayerOne;

            DataPersistenceManager.Instance.LoadPlayerPos(isOne);
        }
    }
}
