using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Sirenix.OdinInspector.Editor.UnityPropertyEmitter;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.I._waitingToActEvent.AddListener(() => spriteRenderer.enabled = false );
        GameManager.I._overwatchEvent.AddListener(() => spriteRenderer.enabled = false);
        GameManager.I._winTheLevelFeedbackEvent.AddListener(() => spriteRenderer.enabled = false);
        GameManager.I._goToMenuEvent.AddListener(() => spriteRenderer.enabled = true);
        MenuManager.I._endTutoEvent.AddListener(() => spriteRenderer.enabled = false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos -9.5f * Vector3.forward;
    }
}
