using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] Rectangle filledLine, feedbackLine, preshootLine; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        preshootLine.Height = (7.57f * 1f - (PlayerMovement.I._dashDistance / GV.GameSO._maxJumpDistance));
    }
}
