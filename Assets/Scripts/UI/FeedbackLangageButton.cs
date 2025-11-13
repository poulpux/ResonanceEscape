using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackLangageButton : MonoBehaviour
{
    bool isActivate = true, canInteract = true;
    [SerializeField] private MMF_Player appearFeedback, desappearFeedback;

    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._menuLangage]._click2DEvent.AddListener(() => ClickOnButton());
    }

    private void ClickOnButton()
    {
        if (!canInteract)
            return;

        isActivate = ! isActivate;
        if (isActivate)
            appearFeedback.PlayFeedbacks();
        else
            desappearFeedback.PlayFeedbacks();

        StartCoroutine(WaitAnimationTime());
    }

    private IEnumerator WaitAnimationTime()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.2f);
        canInteract = true;
    }
}
