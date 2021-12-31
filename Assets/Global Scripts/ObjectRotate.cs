using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [Range(-1000f, 1000f)]
    public float rotX;
    [Range(-1000f, 1000f)]
    public float rotY;
    [Range(-1000f, 1000f)]
    public float rotZ;
    void Update()
    {
        transform.Rotate(transform.rotation * new Vector3(rotX * Time.deltaTime, rotY * Time.deltaTime, rotZ * Time.deltaTime));
    }
}
