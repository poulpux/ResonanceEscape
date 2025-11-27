using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelPrevisualiseMove : MonoBehaviour
{
    [SerializeField] GameObject futurSelf, receptacle;
    [SerializeField] Line line, lineAprès;
    
    void Update()
    {
        if ((GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.OVERWATCH)
            && !GameManager.I._replay
            && PlayerMovement.I._dashDistance < GV.GameSO._maxJumpDistance)
        {
            Vector3 mousePos = new Vector3(
     Mathf.Clamp(UnityEngine.Input.mousePosition.x, 0, Screen.width),
     Mathf.Clamp(UnityEngine.Input.mousePosition.y, 0, Screen.height),
     UnityEngine.Input.mousePosition.z);
            mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            futurSelf.transform.parent.transform.gameObject.SetActive(true);
            Vector3 direction = (worldPos - PlayerMovement.I.transform.position).normalized;
            float distance = Vector3.Distance(worldPos, PlayerMovement.I.transform.position);
            float maxDistance = GV.GameSO._maxJumpDistance - PlayerMovement.I._dashDistance;

            lineAprès.enabled = distance > maxDistance;

            if(lineAprès.enabled)
            {
                lineAprès.Start = futurSelf.transform.localPosition;
                receptacle.transform.position = PlayerMovement.I.transform.position + direction * distance;
                lineAprès.End = receptacle.transform.localPosition;
            }
            if (distance > maxDistance)
                worldPos = PlayerMovement.I.transform.position + direction * maxDistance;
            futurSelf.transform.position = worldPos;
            line.End = futurSelf.transform.localPosition;

        }
        else 
            futurSelf.transform.parent.transform.gameObject.SetActive(false);
    }
}
