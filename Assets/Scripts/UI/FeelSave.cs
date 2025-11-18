using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelSave : MonoBehaviour
{
    [SerializeField] MMF_Player feedbackSave;

    void Start()
    {
        GameManager.I._saveEvent.AddListener(() => { if(!feedbackSave.IsPlaying) feedbackSave.PlayFeedbacks(); });
    }
}
