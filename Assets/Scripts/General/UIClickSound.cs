using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIClickSound : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlayClick()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }
    }
}

