using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    bool canMove, canDie, isDead;
    public bool _lastThingWasAMove;
    public Vector3 startpos, posToGO, lastPos;
    [SerializeField] MMF_Player moveFeedback, inertieFeedback, wallCollisionFeedback, winFeedback;
    [SerializeField] TrailRenderer trailInertie;

    public Rigidbody2D _rigidBody;
    public float _dashDistance;
    public float _timer;
    public  List<Vector2> gostAllFrames = new List<Vector2>();
    public  List<EFeedbackType> gostAllFeedback = new List<EFeedbackType>();
    int indexGhost, indexFeedback;
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            PlayerPrefs.SetFloat(i.ToString(), 99.99f);
        }

        GameManager.I._waitingToActEvent.AddListener(() => { canMove = true; if (_dashDistance >= GV.GameSO._maxJumpDistance) TryInertie();});
        GameManager.I._overwatchEvent.AddListener(() => { StartCoroutine(WaitPlayAnimation()); });
        GameManager.I._winTheLevelFeedbackEvent.AddListener(() => { /*moveFeedback.StopFeedbacks();*/ /*canDie = false; canMove = false; rigidBody.bodyType = RigidbodyType2D.Kinematic; rigidBody.velocity = Vector2.zero; EditorManager.I.F_SetGoodPlayPlayer(); _dashDistance = 0f;*/ ResetLV(); winFeedback.PlayFeedbacks(); });
        GameManager.I._goToMenuEvent.AddListener(() => { gostAllFrames.Clear(); gostAllFeedback.Clear(); canDie = false; canMove = false; _rigidBody.bodyType = RigidbodyType2D.Kinematic; _rigidBody.velocity = Vector2.zero; EditorManager.I.F_SetGoodPlayPlayer(); _dashDistance = 0f; });

        InputSystem_.I._leftClick._event.AddListener(()=> { if (!GameManager.I._replay) TryMove(); });  
        InputSystem_.I._space._event.AddListener(()=> { if (!GameManager.I._replay) TryInertie(); });
        InputSystem_.I._r._event.AddListener(()=> { if (!GameManager.I._replay) ResetLV(); gostAllFrames.Clear(); gostAllFeedback.Clear(); });
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (isDead || GameManager.I._replay)
            return;

        if (_timer < GV.GameSO._pulseIntervale && _lastThingWasAMove)
            _rigidBody.MovePosition(startpos + (_timer / GV.GameSO._pulseIntervale) * (posToGO - startpos));

        if (_lastThingWasAMove)
        {
            _dashDistance += Vector3.Distance(transform.position, lastPos);
            lastPos = transform.position;

            if(_dashDistance >= GV.GameSO._maxJumpDistance)
            {
                moveFeedback.StopFeedbacks();
                gostAllFeedback.Add(EFeedbackType.MOVEFEEDBACKSTOP);
                //_dashDistance -= Vector3.Distance(posToGO, transform.position);
                _dashDistance = GV.GameSO._maxJumpDistance;
                Vector2 inertie = transform.position - startpos;
                _rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
                _lastThingWasAMove = false;
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
            if (_timer < 1.5f)
                return;
            
            if(_timer > GV.GameSO._pulseIntervale + 1.5f)
            {
                GameManager.I._pulseEvent.Invoke();
                _timer = 1.5f;
                _timer = 1.5f;
            }    
            transform.position = gostAllFrames[indexGhost];
            if (indexGhost == gostAllFrames.Count - 1)
            {
                _timer = 0f;
                indexGhost = 0;
                indexFeedback = 0;
                StopInertieFeedback();
                EditorManager.I.F_SetGoodPlayPlayer();
            }
            else
                indexGhost++;

            if (indexFeedback >= gostAllFeedback.Count - 1)
            {
                StopInertieFeedback();
                indexFeedback = 0;
            }

            EFeedbackType feedbackType = gostAllFeedback[indexFeedback];
            while (feedbackType != EFeedbackType.PASS)
            {
                if (feedbackType == EFeedbackType.MOVEFEEDBACKPLAY)
                    moveFeedback.PlayFeedbacks();
                else if (feedbackType == EFeedbackType.MOVEFEEDBACKSTOP)
                    moveFeedback.StopFeedbacks();
                else if (feedbackType == EFeedbackType.INERTIEFEEDBACKPLAY)
                    inertieFeedback.PlayFeedbacks();
                else if (feedbackType == EFeedbackType.INERTIEFEEDBACKSTOP)
                    StopInertieFeedback();
                else if (feedbackType == EFeedbackType.COLLISIONFEEDBACK)
                    wallCollisionFeedback.PlayFeedbacks();

                indexFeedback++;
                feedbackType = gostAllFeedback[indexFeedback];
            }
            indexFeedback++;
        }
        else
        {
            if (GameManager.I._state == EGameState.ACT)
            {
                gostAllFrames.Add(transform.position);
                gostAllFeedback.Add(EFeedbackType.PASS);
            }
        }
    }

    private void TryMove()
    {
        isDead = false;
        if(!canMove || _dashDistance >= GV.GameSO._maxJumpDistance) return;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _lastThingWasAMove = true;
        _rigidBody.gravityScale = 0f;
        Vector3 mousePos = new Vector3(
     Mathf.Clamp(UnityEngine.Input.mousePosition.x, 0, Screen.width),
     Mathf.Clamp(UnityEngine.Input.mousePosition.y, 0, Screen.height),
     UnityEngine.Input.mousePosition.z
 );
        StopInertieFeedback(); 
        gostAllFeedback.Add(EFeedbackType.INERTIEFEEDBACKSTOP);
        moveFeedback.PlayFeedbacks();
        gostAllFeedback.Add(EFeedbackType.MOVEFEEDBACKPLAY);
        mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        startpos = transform.position;
        posToGO = worldPos;
        lastPos = transform.position;
        canMove = false;
        _rigidBody.velocity = Vector2.zero;
        _timer = 0f;

        GameManager.I._playerActEvent.Invoke();
    }

    private void TryInertie()
    {
        isDead = false;
        if (!canMove) return;

        inertieFeedback.PlayFeedbacks();
        gostAllFeedback.Add(EFeedbackType.INERTIEFEEDBACKPLAY);
        moveFeedback.StopFeedbacks();
        gostAllFeedback.Add(EFeedbackType.MOVEFEEDBACKSTOP);
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.gravityScale = 1f;
        if (_lastThingWasAMove)
        {
            Vector2 inertie = posToGO - startpos;
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            _lastThingWasAMove= false;
        }
        GameManager.I._playerActEvent.Invoke();
    }

    private void ResetLV()
    {
        if (!(GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._state == EGameState.OVERWATCH || GameManager.I._replay))
            return;

        moveFeedback.StopFeedbacks();
        if (!GameManager.I._replay)
            gostAllFeedback.Add(EFeedbackType.MOVEFEEDBACKSTOP);
        StopInertieFeedback();
        canDie = false;
        canMove= false;
        isDead = true;
        _lastThingWasAMove = false;
        _dashDistance = 0f;
        indexGhost = 0;
        indexFeedback = 0;
        _timer = 0f;
        StartCoroutine(WaitPlayAnimation());
        _rigidBody.bodyType = RigidbodyType2D.Kinematic; 
        _rigidBody.velocity = Vector2.zero; 
        EditorManager.I.F_SetGoodPlayPlayer();
    }

    private IEnumerator WaitPlayAnimation()
    {
        yield return new WaitForSecondsRealtime(GV.GameSO._pulseIntervale);
        canMove = true; canDie = true; isDead = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == GV.TagSO._gameWallCollision)
        {
            if (_lastThingWasAMove || _rigidBody.velocity.magnitude > 12f)
            {
                wallCollisionFeedback.PlayFeedbacks();
                if(!GameManager.I._replay)
                    gostAllFeedback.Add(EFeedbackType.COLLISIONFEEDBACK);
            }
        }
        if(collision.transform.tag == GV.TagSO._gameWallCollision && _lastThingWasAMove)
        {
            moveFeedback.StopFeedbacks();
            //_dashDistance -= Vector3.Distance(posToGO, transform.position);
            Vector2 inertie = posToGO - startpos;
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.AddForce(inertie * 4f, ForceMode2D.Impulse);
            _lastThingWasAMove = false;
        }
        else if(collision.transform.tag == GV.TagSO._gameDie && canDie && !GameManager.I._replay)
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
            if (!GameManager.I._replay)
            {
                gostAllFeedback.Add(EFeedbackType.MOVEFEEDBACKSTOP);
                gostAllFeedback.Add(EFeedbackType.INERTIEFEEDBACKSTOP);
                gostAllFeedback.Add(EFeedbackType.PASS);
                GameManager.I._winTheLevelEvent.Invoke();
                GameManager.I._winTheLevelFeedbackEvent.Invoke();
            }
        }
        else if(collision.transform.tag == GV.TagSO._gameInertieBoost)
        {
            if(!_lastThingWasAMove)
                _rigidBody.AddForce(_rigidBody.velocity, ForceMode2D.Impulse);
        }
        else if(collision.transform.tag == GV.TagSO._gameStar)
        {
            //Super, t'as gagné une étoile
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == GV.TagSO._gameBlackHole)
        {
            if (_rigidBody == null || _rigidBody.isKinematic) return;

            // Direction vers le centre
            Vector2 direction = (Vector2)collision.transform.position - _rigidBody.position;
            float distance = direction.magnitude;

            // On évite les divisions nulles
            distance = Mathf.Max(distance, 0.1f);

            // Intensité de la force (1 / distance^n)
            float forceMagnitude = GV.GameSO._blackHolePower / Mathf.Pow(distance, 2f);

            // Applique la force proportionnelle à la distance et à la direction
            _rigidBody.AddForce(direction.normalized * forceMagnitude, ForceMode2D.Force);
        }

        //BlackHole
    }

    private void StopInertieFeedback()
    {
        inertieFeedback.StopFeedbacks();
        if (!GameManager.I._replay)
            gostAllFeedback.Add(EFeedbackType.INERTIEFEEDBACKSTOP);
        trailInertie.Clear();
    }
}
