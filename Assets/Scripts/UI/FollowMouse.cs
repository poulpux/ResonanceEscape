using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Sirenix.OdinInspector.Editor.UnityPropertyEmitter;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject trail;

    bool onTuto;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    GameManager.I._waitingToActEvent.AddListener(() => { if (!onTuto) Active(false); });
    //    GameManager.I._overwatchEvent.AddListener(() => { if (!onTuto) Active(false); });
    //    GameManager.I._winTheLevelFeedbackEvent.AddListener(() => Active(false));
    //    GameManager.I._goToMenuEvent.AddListener(() => { onTuto = false;  Active(true); });
    //    MenuManager.I._startTutoEvent.AddListener(() => { print("passe"); onTuto = true; Active(true); });
    //    MenuManager.I._endTutoEvent.AddListener(() => { onTuto = false; Active(false); });
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos -9.5f * Vector3.forward;
    }

    private void Active(bool active)
    {
        spriteRenderer.enabled = active;
        trail.SetActive(active);
    }
}
