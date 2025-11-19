using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelPassLevelTriangle : MonoBehaviour
{
    [SerializeField] Triangle triangle;
    [SerializeField] MMF_Player feedbackClick;

    bool survole;

    [SerializeField] Color colorUnclicked, colorClicked;

    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => feedbackClick.PlayFeedbacks());
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => { survole = true; triangle.Color = colorClicked; });
    }

    private void FixedUpdate()
    {
        if(!survole)
            triangle.Color = colorUnclicked;

        survole = false;
    }
}
