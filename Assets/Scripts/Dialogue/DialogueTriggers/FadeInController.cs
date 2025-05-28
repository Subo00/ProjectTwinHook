using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInController : MonoBehaviour {
    public Image fader;
    bool fadedIn = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !fadedIn) {
            fader.DOFade(0, 0.5f);
            fadedIn = true;
        }
    }
}
