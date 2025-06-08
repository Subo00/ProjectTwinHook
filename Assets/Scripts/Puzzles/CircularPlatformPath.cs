using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPlatformPath : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float radius = 5f;
    public float offset = 0.25f;
    private float angle = 1f;

    void Update()
    {
        float x = target.position.x + Mathf.Cos(angle+ offset) *radius;
        float y = target.position.y + Mathf.Sin(angle+ offset) * radius;
        float z = target.position.z;

        transform.position = new Vector3(x, y, z);

        angle += speed * Time.deltaTime;

    }
}
