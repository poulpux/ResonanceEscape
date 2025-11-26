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

        if (UnityEngine.Input.GetMouseButtonDown(0))
            GoNextPos();

        if(UnityEngine.Input.GetMouseButtonDown(1))
            GoBackPos();

        float multiplicateur = 10f;

        transform.eulerAngles = new Vector3(Mathf.Clamp((UnityEngine.Input.mousePosition.y - 960f) / 1920f, -1f, 1f)*-multiplicateur, Mathf.Clamp((UnityEngine.Input.mousePosition.x - 540f) / 1080f,-1f, 1f)*multiplicateur, 0f);
    }

    private void GoNextPos()
    {
        if (animateCurve)
            return;
        SoundManager.I.F_PlaySound(GV.SoundSO._dropTileEditor);
        currentPos = currentPos >= allCamPos.Count-1 ? 0 : currentPos+1;
        if(currentPos == 0)
        {
            SceneManager.LoadScene(0);
            SoundManager.I.F_PlayMusic(GV.SoundSO._menuMusic, true);
            Destroy(this.gameObject);
            return;
        }
        curve = new AnimatingCurve(transform.position, (Vector3) allCamPos[currentPos] + Vector3.forward * -10, timeAnimation, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
        animateCurve = true;
    }

    private void GoBackPos()
    {
        if (animateCurve)
            return;
        SoundManager.I.F_PlaySound(GV.SoundSO._eraseTileEditor);
        currentPos = currentPos <= 0 ? allCamPos.Count-1 : currentPos-1;
        curve = new AnimatingCurve(transform.position, (Vector3)allCamPos[currentPos] + Vector3.forward * -10, timeAnimation, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
        animateCurve = true;
    }
}
