using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackParameter : MonoBehaviour
{
    [SerializeField] MMF_Player selectFeedback, unselectFeedback;
    bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => FeedbackPlay());
        GameManager.I._goToMenuEvent.AddListener(() => Unselect());
        GameManager.I._playPlayModeEvent.AddListener(() => Unselect());
        GameManager.I._enterInEditModeEvent.AddListener(() => Unselect());
        GameManager.I._enterInEditModePastEvent.AddListener(() => Unselect());
    }

    private void FeedbackPlay()
    {
        if (isSelected)
        {
            isSelected = false;
            unselectFeedback.PlayFeedbacks();
        }
        else
        {
            isSelected = true;
            selectFeedback.PlayFeedbacks();
        }
    }

    private void Unselect()
    {
        isSelected = false;
        unselectFeedback.PlayFeedbacks();
    }
}
