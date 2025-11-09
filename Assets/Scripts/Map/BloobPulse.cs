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
                invoque = Instantiate(GV.PrefabSO._murBloobPlein);
            else
                invoque = Instantiate(GV.PrefabSO._bloobPlein);
        }
        else
        {
            if (wall)
                invoque = Instantiate(GV.PrefabSO._murBloobVide);
            else
                invoque = Instantiate(GV.PrefabSO._bloobVide);
        }

        invoque.transform.position = transform.position;
        EditorManager.I._allObject.Add(invoque);
        EditorManager.I._allObject.Remove(gameObject);
        Destroy(gameObject);
    }
}
