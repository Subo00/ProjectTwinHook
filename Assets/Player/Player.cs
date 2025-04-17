using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ThirdPersonMovement
{
    public bool isPlayerOne;

    protected override void Start()
    {
        horizontal = isPlayerOne ? "Horizontal 1" : "Horizontal 2";
        vertical = isPlayerOne ? "Vertical 1" : "Vertical 2";
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
}
