using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelPassLevelTriangle : MonoBehaviour
{
    [SerializeField] Triangle triangle;
    [SerializeField] MMF_Player feedbackClick;

    float timer;

    [SerializeField] Color colorUnclicked, colorClicked;

    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => { SoundManager.I.F_PlaySound(GV.SoundSO._clicSurvole); feedbackClick.PlayFeedbacks(); });
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => 
        {
            if (triangle.Color != colorClicked && timer > 0.2f) 
            { 
                SoundManager.I.F_PlaySound(GV.SoundSO._boutonSurvole); 
                triangle.Color = colorClicked; 
            } });
            timer = 0f;
    }

    private void FixedUpdate()
    {
        if (timer<0.2f)
        {
            triangle.Color = colorUnclicked;
        }

        timer += Time.deltaTime;
    }
}
