using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    public AudioClip footstepLeft;
    public AudioClip footstepRight;

    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFoot() {
        SoundManager.PlaySound(SoundType.WALK);
    }

    public void PlayStepRight() {
        audioSource.PlayOneShot(footstepRight);
    }

    public void PlayStepLeft() {
        audioSource.PlayOneShot(footstepLeft);
    }
}
