using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public AnimationCurve curve = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.5f, 0.8f),
    new Keyframe(1f, 1f)
);
    // Start is called before the first frame update
    void Start()
    {
        print(GV.TagSO._quitParamter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
