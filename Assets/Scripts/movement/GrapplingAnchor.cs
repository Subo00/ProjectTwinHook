using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinHookController
{
    public class GrapplingAnker : Interactable
    {

        protected override void OnUpdate()
        {
            CommonLogic();
        }
        //trigger collider
        //void OnTriggerEnter(Collider other)
        //{
        //    //let it glow here
        //    Debug.Log("anchor position: " + this.transform.position);
        //}

    }

}
