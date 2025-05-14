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
            other.GetComponent<Player>().Die();
        }
    }
}
