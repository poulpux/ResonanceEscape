using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvoleMarketingButton : MonoBehaviour
{
    [SerializeField] MMF_Player feedbackSurvole, feedbackSurvoleBack;
    bool survole;

    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => Survole());
    }

    private void FixedUpdate()
    {
        VerifSurvoleback();
    }

    private void Survole()
    {
        survole = true;
        if (feedbackSurvole != null && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && transform.localScale.x == 1f /*&& carré1.transform.eulerAngles.z == 0f*/)
            feedbackSurvole.PlayFeedbacks();
    }

    private void VerifSurvoleback()
    {
        if (!survole && feedbackSurvole != null && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && transform.localScale.x == 1f /*&& Mathf.Abs(carré1.transform.eulerAngles.z - 45f) < 0.1f*/)
        {
            feedbackSurvoleBack.PlayFeedbacks();
        }
        survole = false;
    }
}
