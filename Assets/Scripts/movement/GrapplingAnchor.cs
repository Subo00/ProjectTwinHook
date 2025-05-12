using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinHookController
{
    public class GrapplingAnker : Interactable
    {
        protected override void OnUpdate()  {
            CommonLogic();
        }

        private void OnDisable() {
            UpdateManager.Instance.RemoveUpdatable(this);
            uiManager.HideInteraction(); 
        }
    }

}
