using System.Collections;
using System.Collections.Generic;
using TwinHookController;
using UnityEngine;

namespace TwinHookController
{

    public class Player : PlayerMovement
    {
        public bool isPlayerOne;

        protected override void Start()
        {
            horizontal = isPlayerOne ? "Horizontal 1" : "Horizontal 2";
            jump = isPlayerOne ? "Jump 1" : "Jump 2";
            duck = isPlayerOne ? "Duck 1" : "Duck 2";
            grapple = isPlayerOne ? "Grapple 1" : "Grapple 2";
        }


    }
}
