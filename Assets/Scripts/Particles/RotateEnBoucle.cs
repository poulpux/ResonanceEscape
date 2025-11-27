using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnBoucle : MonoBehaviour
{
    [SerializeField] bool blackHole;
    AnimatingCurve curveColor;
    [SerializeField] Color color1, color2;
    [SerializeField] List<Disc> allDisc = new List<Disc>();
    private void Start()
    {
        if (color1 != null && color2 != null)
            curveColor = new AnimatingCurve(new Vector3(color1.r, color1.g, color1.b), new Vector3(color2.r, color2.g, color2.b), 0.5f, GRAPH.LINEAR, INANDOUT.INOUT, LOOP.PINGPONG);
    }

    void FixedUpdate()
    {
        if(color1 != null)
        {
            Vector3 colorEnVector = Vector3.zero;
            Tools.PlayCurve(ref curveColor, ref colorEnVector);
            foreach (var item in allDisc)
            {
                if(Tools.isCurveFinish(curveColor))
                {
                    if(curveColor.beginValue == new Vector3(color1.r, color1.g, color1.b))
                        curveColor = new AnimatingCurve(new Vector3(color2.r, color2.g, color2.b), new Vector3(color1.r, color1.g, color1.b), 0.5f, GRAPH.LINEAR, INANDOUT.INOUT, LOOP.PINGPONG);
                    else
                        curveColor = new AnimatingCurve(new Vector3(color1.r, color1.g, color1.b), new Vector3(color2.r, color2.g, color2.b), 0.5f, GRAPH.LINEAR, INANDOUT.INOUT, LOOP.PINGPONG);
                }
                //item.Color = new Color(curveColor.endValue.x, curveColor.endValue.y, curveColor.endValue.z, 1f);
                else
                    item.Color = new Color(colorEnVector.x, colorEnVector.y, colorEnVector.z, 1f);
            }
        }
        transform.eulerAngles += Vector3.forward * (blackHole ?0.05f : 0.025f);
    }
}
