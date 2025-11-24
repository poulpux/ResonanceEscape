using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnBoucle : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.eulerAngles += Vector3.forward * 0.025f;
    }
}
