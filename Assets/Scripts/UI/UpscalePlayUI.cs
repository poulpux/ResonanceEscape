using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpscalePlayUI : MonoBehaviour
{
    [SerializeField] MMF_Player upscaleFeedback;

    private void OnEnable()
    {
        upscaleFeedback.PlayFeedbacks();
    }
}
