using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrreditCameraController : MonoBehaviour
{
    int currentPos;
    public List<Vector2> allCamPos = new List<Vector2>();

    float timeAnimation = 0.8f;
    bool animateCurve;
    AnimatingCurve curve = new AnimatingCurve();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animateCurve)
        {
            Vector3 currentPos = transform.position;
            Tools.PlayCurve(ref curve, ref currentPos);
            transform.position = currentPos;

            if(Tools.isCurveFinish(curve))
                animateCurve = false;
        }

        if (Input.GetMouseButtonDown(0))
            GoNextPos();

        if(Input.GetMouseButtonDown(1))
            GoBackPos();

        float multiplicateur = 10f;
        transform.eulerAngles = new Vector3(Mathf.Clamp((Input.mousePosition.y - 960f) / 1920f, -1f, 1f)*-multiplicateur, Mathf.Clamp((Input.mousePosition.x - 540f) / 1080f,-1f, 1f)*multiplicateur, 0f);
    }

    private void GoNextPos()
    {
        if (animateCurve)
            return;
        currentPos = currentPos >= allCamPos.Count-1 ? 0 : currentPos+1;
        if(currentPos == 0)
        {
            SceneManager.LoadScene(0);
            Destroy(this.gameObject);
            return;
        }
        curve = new AnimatingCurve(transform.position, (Vector3) allCamPos[currentPos] + Vector3.forward * -10, timeAnimation, GV._feedbackSO._type, INANDOUT.IN, LOOP.CLAMP);
        animateCurve = true;
    }

    private void GoBackPos()
    {
        if (animateCurve)
            return;
        currentPos = currentPos <= 0 ? allCamPos.Count-1 : currentPos-1;
        curve = new AnimatingCurve(transform.position, (Vector3)allCamPos[currentPos] + Vector3.forward * -10, timeAnimation, GV._feedbackSO._type, INANDOUT.IN, LOOP.CLAMP);
        animateCurve = true;
    }
}
