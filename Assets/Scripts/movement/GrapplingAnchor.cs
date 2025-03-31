using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingAnker : MonoBehaviour, Grapple
{
    //raycast stuff
    public void Grapple()
    {
        Debug.Log("grapple");
    }

    //trigger collider
    void OnTriggerEnter(Collider other)
    {
        //let it glow here
        Debug.Log("anchor position: " + this.transform.position);
    }

}
