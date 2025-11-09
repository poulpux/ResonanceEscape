using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloobPulse : MonoBehaviour
{
    public bool vide, wall;


    void Start()
    {
        GameManager.I._pulseEvent.AddListener(() => Change());
        //InputSystem_.I._r._event.AddListener(() => { if (GameManager.I._state != EGameState.EDITOR) Destroy(this.gameObject); });
    }

    private void Change()
    {
        GameObject invoque = null;
        if (vide)
        {
            if (wall)
                invoque = Instantiate(GV.PrefabSO._murBloobPlein, EditorManager.I._shapes.transform);
            else
                invoque = Instantiate(GV.PrefabSO._bloobPlein, EditorManager.I._shapes.transform);
        }
        else
        {
            if (wall)
                invoque = Instantiate(GV.PrefabSO._murBloobVide, EditorManager.I._shapes.transform);
            else
                invoque = Instantiate(GV.PrefabSO._bloobVide, EditorManager.I._shapes.transform);
        }

        invoque.transform.position = transform.position;
        //invoque.transform.localScale = transform.parent.localScale;
        EditorManager.I._allObject.Add(invoque);
        EditorManager.I._allObject.Remove(gameObject);
        Destroy(gameObject);
    }
}
