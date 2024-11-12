using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    private Vector3 rotateSpeed = new Vector3(0, 0, -0.05f);

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(rotateSpeed);
    }
}
