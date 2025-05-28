using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace TwinHookController
{

    public class Player : PlayerMovement, IDataPersistence
    {
        public bool isPlayerOne;

        protected override void Start() {
            base.Start();
            horizontal = isPlayerOne ? "Horizontal 1" : "Horizontal 2";
            jump = isPlayerOne ? "Jump 1" : "Jump 2";
            duck = isPlayerOne ? "Duck 1" : "Duck 2";
            grapple = isPlayerOne ? "Grapple 1" : "Grapple 2";

            if (isController)
            {
                jump += " (Controller)";
                horizontal += " (Controller)";
                duck += " (Controller)";
                grapple += " (Controller)";
            }

            if (isPlayerOne){
                DataPersistenceManager.Instance.PlayerOne = this;
            }
            else{
                DataPersistenceManager.Instance.PlayerTwo = this;
            }
        }

        public void LoadData(GameData data) {
            //controller.enabled = false;
            this.transform.position = isPlayerOne ? data.playerOnePosition : data.playerTwoPosition;
            //this.transform.rotation = data.playerRotation;
            //controller.enabled = true;
        }

        public void SaveData(ref GameData data) {
           
        }

        public void Die() {
            SoundManager.PlaySound(SoundType.DEATH);
            DataPersistenceManager.Instance.LoadPlayerPos(isPlayerOne);
        }
    }
}
