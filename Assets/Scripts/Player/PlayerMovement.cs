using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    bool canMove, canDie;
    bool lastThingWasAMove, isDead;
    public Vector3 startpos, posToGO, lastPos;
    [SerializeField] MMF_Player moveFeedback;

    Rigidbody2D rigidBody;
    public float _dashDistance;
    float timer;
   public  List<Vector2> gostAllFrames = new List<Vector2>();
    int indexGhost;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        GameManager.I._waitingToActEvent.AddListener(() => { canMove = true; });
        GameManager.I._overwatchEvent.AddListener(() => { StartCoroutine(WaitPlayAnimation()); });
        GameManager.I._winTheLevelEvent.AddListener(() => { /*moveFeedback.StopFeedbacks();*/ /*canDie = false; canMove = false; rigidBody.bodyType = RigidbodyType2D.Kinematic; rigidBody.velocity = Vector2.zero; EditorManager.I.F_SetGoodPlayPlayer(); _dashDistance = 0f;*/ ResetLV(); });
        GameManager.I._goToMenuEvent.AddListener(() => { gostAllFrames.Clear(); canDie = false; canMove = false; rigidBody.bodyType = RigidbodyType2D.Kinematic; rigidBody.velocity = Vector2.zero; EditorManager.I.F_SetGoodPlayPlayer(); _dashDistance = 0f; });

        InputSystem_.I._leftClick._event.AddListener(()=> { if (!GameManager.I._replay) TryMove(); });  
        InputSystem_.I._space._event.AddListener(()=> { if (!GameManager.I._replay) TryInertie(); });
        InputSystem_.I._r._event.AddListener(()=> { if (!GameManager.I._replay) ResetLV(); gostAllFrames.Clear(); });


        //moveFeedback.FeedbacksList[0].FeedbackDuration = GV.GameSO._pulseIntervale;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isDead || GameManager.I._replay)
            return;

        if (timer < GV.GameSO._pulseIntervale && lastThingWasAMove)
            rigidBody.MovePosition(startpos + (timer / GV.GameSO._pulseIntervale) * (posToGO - startpos));

        if (lastThingWasAMove)
        {
            _dashDistance += Vector3.Distance(transform.position, lastPos);
            lastPos = transform.position;

            if(_dashDistance >= GV.GameSO._maxJumpDistance)
            {
                moveFeedback.StopFeedbacks();
                //_dashDistance -= Vector3.Distance(posToGO, transform.position);
                _dashDistance = GV.GameSO._maxJumpDistance;
                Vector2 inertie = transform.position - startpos;
                print(inertie);
                rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
                lastThingWasAMove = false;
            }
        }
    }

    private void FixedUpdate()
    {
        TryReplay();
    }

    private void TryReplay()
    {
        if (GameManager.I._replay)
        {
            if (timer < 0.5f)
                return;
            
            transform.position = gostAllFrames[indexGhost];
            indexGhost = indexGhost == gostAllFrames.Count - 1 ? 0 : indexGhost += 1;
        }
        else
        {
            if (GameManager.I._state == EGameState.ACT)
            {
                gostAllFrames.Add(transform.position);
                print("print");
            }
            print(GameManager.I._state);
        }
    }

    private void TryMove()
    {
        isDead = false;
        if(!canMove && _dashDistance < GV.GameSO._maxJumpDistance) return;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        lastThingWasAMove = true;
        rigidBody.gravityScale = 0f;
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        startpos = transform.position;
        posToGO = worldPos;
        lastPos = transform.position;
        canMove = false;
        //if(!GameManager.I._replay)
        //    gostActionList.Add(posToGO);
        rigidBody.velocity = Vector2.zero;

        
        //moveFeedback.transform.position = posToGO;
        //moveFeedback.PlayFeedbacks();
        timer = 0f;

        GameManager.I._playerActEvent.Invoke();
    }
    private void TryMove(Vector2 posToGO)
    {
        isDead = false;
        if(!canMove && _dashDistance < GV.GameSO._maxJumpDistance) return;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        lastThingWasAMove = true;
        rigidBody.gravityScale = 0f;
        startpos = transform.position;
        lastPos = transform.position;
        canMove = false;
        rigidBody.velocity = Vector2.zero;
        this.posToGO = posToGO;
        
        //moveFeedback.transform.position = posToGO;
        //moveFeedback.PlayFeedbacks();
        timer = 0f;

        GameManager.I._playerActEvent.Invoke();
    }

    private void TryInertie()
    {
        isDead = false;
        if (!canMove) return;

        //if(!GameManager.I._replay)
        //    gostActionList.Add(Vector2.one * -99f);
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 1f;
        if (lastThingWasAMove)
        {
            Vector2 inertie = posToGO - startpos;
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            lastThingWasAMove= false;
        }
        GameManager.I._playerActEvent.Invoke();
    }

    private void ResetLV()
    {
        if (!(GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._state == EGameState.OVERWATCH || GameManager.I._replay))
            return;

        moveFeedback.StopFeedbacks();
        canDie = false;
        canMove= false;
        isDead = true;
        lastThingWasAMove = false;
        _dashDistance = 0f;
        indexGhost = 0;
        timer = 0f;
        StartCoroutine(WaitPlayAnimation());
        rigidBody.bodyType = RigidbodyType2D.Kinematic; 
        rigidBody.velocity = Vector2.zero; 
        EditorManager.I.F_SetGoodPlayPlayer();
    }

    private IEnumerator WaitPlayAnimation()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        canMove = true; canDie = true; isDead = false; print("passe 2");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == GV.TagSO._gameWallCollision && lastThingWasAMove)
        {
            moveFeedback.StopFeedbacks();
            //_dashDistance -= Vector3.Distance(posToGO, transform.position);
            Vector2 inertie = posToGO - startpos;
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            lastThingWasAMove = false;
        }
        else if(collision.transform.tag == GV.TagSO._gameDie && canDie)
        {
            isDead = true;
            InputSystem_.I._r._event.Invoke();
            //Feedback You Dead
        }

       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == GV.TagSO._gameWinCondition)
        {
            //écran de victoire et choix entre plein de trucs
            //Je veux juste revenir au menu pour le moment
            GameManager.I._winTheLevelEvent.Invoke();
        }
        else if(collision.transform.tag == GV.TagSO._gameInertieBoost)
        {
            if(!lastThingWasAMove)
                rigidBody.AddForce(rigidBody.velocity, ForceMode2D.Impulse);
        }
        else if(collision.transform.tag == GV.TagSO._gameStar)
        {
            //Super, t'as gagné une étoile
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == GV.TagSO._gameBlackHole)
        { }

        //BlackHole
    }
}
