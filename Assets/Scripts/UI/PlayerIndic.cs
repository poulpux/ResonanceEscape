using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndic : MonoBehaviour
{
    [SerializeField] MMF_Player feedbackPlayerIndic;

    void Start()
    {
        GameManager.I._playPlayModeEvent.AddListener(() => ActivationCondition());
        GameManager.I._playerActEvent.AddListener(() => feedbackPlayerIndic.StopFeedbacks());
    }

    private void ActivationCondition()
    {
        feedbackPlayerIndic.PlayFeedbacks();
    }
}
