using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class OneTimeTrigger : MonoBehaviour {
    public MovingPlatform[] platforms;

    private void Start() {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            for(int i = 0; i < platforms.Length; i++) {
                platforms[i].SetBool(true);
            }
        }
    }

}
