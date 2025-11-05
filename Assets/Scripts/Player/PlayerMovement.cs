using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool canMove, canDie;
    bool lastThingWasAMove;
    public Vector3 startpos, posToGO;
    [SerializeField] MMF_Player moveFeedback;

    Rigidbody2D rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        InputSystem_.I._leftClick._event.AddListener(()=>TryMove());  
        GameManager.I._waitingToActEvent.AddListener(() => { canMove = true; });
        GameManager.I._overwatchEvent.AddListener(() => { StartCoroutine(WaitPlayAnimation()); });
        InputSystem_.I._space._event.AddListener(()=>TryInertie());

        moveFeedback.FeedbacksList[0].FeedbackDuration = GV.GameSO._pulseIntervale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TryMove()
    {
        if(!canMove) return;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        lastThingWasAMove = true;
        rigidBody.gravityScale = 0f;
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        startpos = transform.position;
        posToGO = worldPos;
        canMove = false;
        moveFeedback.transform.position = posToGO;
        moveFeedback.PlayFeedbacks();

        GameManager.I._playerActEvent.Invoke();
    }

    private void TryInertie()
    {
        if (!canMove) return;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 1f;
        if (lastThingWasAMove)
        {
            Vector2 inertie = posToGO - startpos;
            rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            lastThingWasAMove= false;
        }
        GameManager.I._playerActEvent.Invoke();
    }

    private IEnumerator WaitPlayAnimation()
    {
        yield return new WaitForSeconds(1f);
        canMove = true; canDie = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            print("collision2");
        if(collision.transform.tag == GV.TagSO._gameWallCollision && lastThingWasAMove)
        {
            print("collision");
            moveFeedback.StopFeedbacks();
            Vector2 inertie = posToGO - startpos;
            rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            lastThingWasAMove = false;
        }
    }
}
