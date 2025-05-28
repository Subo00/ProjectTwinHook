using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPusher : MovingPlatform {
    protected override void Start() {
        base.Start();
        endPos += startPos;
    }
}
