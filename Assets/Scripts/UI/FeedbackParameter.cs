using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackParameter : MonoBehaviour
{
    [SerializeField] MMF_Player selectFeedback, unselectFeedback;
    [SerializeField] GameObject objet1, objet2;
    bool isSelected;
    Vector3 basepos;
    // Start is called before the first frame update
    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => FeedbackPlay());
        GameManager.I._goToMenuEvent.AddListener(() => Unselect());
        GameManager.I._playPlayModeEvent.AddListener(() => Unselect());
        GameManager.I._enterInEditModeEvent.AddListener(() => Unselect());
        GameManager.I._enterInEditModePastEvent.AddListener(() => Unselect());
        basepos = objet1.transform.position;
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

    private void OnEnable()
    {
        if (basepos == Vector3.zero)
            return;
        objet1.transform.position = basepos;
        objet2.transform.position = basepos - 1f *Vector3.up;
    }
}
