using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] Rectangle filledLine, feedbackLine, preshootLine;
    [SerializeField] MMF_Player feedbackAnticipate;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InputSystem_.I._r._event.AddListener(()=> preshootLine.Height = 7.57f);
    //}



    // Update is called once per frame
    void Update()
    {
        if (GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._replay)
            preshootLine.Height = (7.57f * (1f - (Mathf.Clamp01(PlayerMovement.I._dashDistance / GV.GameSO._maxJumpDistance))));
        else
            preshootLine.Height = 7.57f;
    }
}
