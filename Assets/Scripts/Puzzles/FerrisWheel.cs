using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    public CircularPlatformPath[] platforms;
    private float radsInCircle = 2.0f * Mathf.PI;
    public float speed = 3f;
    public float radius = 5f;

    void Start() {
        if(platforms.Length == 0) {
            return;
        } else {
            for(int i = 0; i < platforms.Length; i++) {
                platforms[i].speed = speed;
                platforms[i].radius = radius;
                platforms[i].target = this.transform;
                float tmp = (float)i / platforms.Length;
                platforms[i].offset = radsInCircle * tmp;
                Debug.Log(platforms[i].offset);
            }
        }
    }

}
