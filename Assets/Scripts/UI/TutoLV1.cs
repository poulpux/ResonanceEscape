using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoLV1 : MonoBehaviour
{
    [SerializeField] GameObject text1, text2;

    void Start()
    {
        GroopedActivation(false);
        GameManager.I._playPlayModeEvent.AddListener(()=> ActivationCondition());
        GameManager.I._playerActEvent.AddListener(()=> GroopedActivation(false));
    }

    private void ActivationCondition()
    {
        GroopedActivation(MenuManager.I._indexMapPlayMode == 0);
    }

    private void GroopedActivation(bool active)
    {
        text1.SetActive(active);
        text2.SetActive(active);
    }
}
