using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5.0f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }

}
