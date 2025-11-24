using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptativeScale : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = transform.parent.localScale;
    }

    //private void OnEnable()
    //{
    //    transform.localScale = transform.parent.localScale;
    //}
}
