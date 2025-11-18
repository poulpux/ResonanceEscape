using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelPassLevelTriangle : MonoBehaviour
{
    [SerializeField] Triangle triangle;
    [SerializeField] MMF_Player feedbackSurvole, feedbackClick;
    // Start is called before the first frame update
    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => feedbackClick.PlayFeedbacks());
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => feedbackSurvole.PlayFeedbacks());
    }
}
