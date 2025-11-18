using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FeelPastCodeError : MonoBehaviour
{
    [SerializeField] MMF_Player feedback;

    void Start()
    {
        GameManager.I._pastErrorEvent.AddListener(() => feedback.PlayFeedbacks());
    }
}
