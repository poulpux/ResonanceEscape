using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBattement : MonoBehaviour
{
    Material mat;
    AnimatingCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        ResetCurve();
        mat.SetFloat("_GradBoostX", curve.beginValueF);
        GameManager.I._goToMenuEvent.AddListener(()=>ResetCurve());
        InputSystem_.I._r._event.AddListener(()=> ResetCurve());
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.I._state == EGameState.ACT)
        {
            float value = 0f;
            Tools.PlayCurve(ref curve, ref value);
            mat.SetFloat("_GradBoostX", value);
        }
    }

    private void ResetCurve()
    {
        curve = new AnimatingCurve(0.35f, 1.08f, 0.15f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.PINGPONG);
        mat.SetFloat("_GradBoostX", curve.beginValueF);
    }
}
