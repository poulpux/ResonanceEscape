using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] Rectangle filledLine, feedbackLine, preshootLine;
    //[SerializeField] MMF_Player feedbackJump;

    AnimatingCurve curve;

    //Preshoot line (plus claire)
    //feedbackLine (plus foncée)

    private void Start()
    {
        GameManager.I._goToMenuEvent.AddListener(() => { preshootLine.Height = 7.57f; feedbackLine.Height = 7.57f; });
        InputSystem_.I._r._event.AddListener(() => { preshootLine.Height = 7.57f; feedbackLine.Height = 7.57f; });
        GameManager.I._playerActEvent.AddListener(() => curve = new AnimatingCurve(feedbackLine.Height, preshootLine.Height, GV.GameSO._pulseIntervale, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I._state == EGameState.ACT)
        {
            //if (PlayerMovement.I._lastThingWasAMove)
            //{
                float height = 0f;
                Tools.PlayCurve(ref curve, ref height);
                feedbackLine.Height = height;
                if (Tools.isCurveFinish(curve))
                    feedbackLine.Height = curve.endValueF;
            //}
        }
        //La ligne la plus claire
        if(GameManager.I._state == EGameState.OVERWATCH || GameManager.I._state == EGameState.WAITINGACTION)
        {
            float baseHeight = (7.57f * (1f - (Mathf.Clamp01(PlayerMovement.I._dashDistance / GV.GameSO._maxJumpDistance))));
            preshootLine.Height = baseHeight;
            feedbackLine.Height = baseHeight;


            Vector3 mousePos = new Vector3(
            Mathf.Clamp(UnityEngine.Input.mousePosition.x, 0, Screen.width),
            Mathf.Clamp(UnityEngine.Input.mousePosition.y, 0, Screen.height),
            UnityEngine.Input.mousePosition.z);
            mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 direction = (worldPos - PlayerMovement.I.transform.position).normalized;
            float distance = Vector3.Distance(worldPos, PlayerMovement.I.transform.position);
            float maxDistance = GV.GameSO._maxJumpDistance - PlayerMovement.I._dashDistance;

            if (distance > maxDistance)
                preshootLine.Height = 0f;
            else
                preshootLine.Height -= distance;



        }

        //if (GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._replay)
        //    preshootLine.Height = (7.57f * (1f - (Mathf.Clamp01(PlayerMovement.I._dashDistance / GV.GameSO._maxJumpDistance))));
        //else
        //    preshootLine.Height = 7.57f;
    }

}
