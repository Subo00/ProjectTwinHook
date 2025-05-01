using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinHookController
{
    public class GrapplingAnker : MonoBehaviour
    {


        //trigger collider
        void OnTriggerEnter(Collider other)
        {
            //let it glow here
            Debug.Log("anchor position: " + this.transform.position);
        }

    }

}
