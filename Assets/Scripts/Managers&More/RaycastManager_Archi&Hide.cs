using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public partial class RaycastManager_
{
    public class RaycastTag
    {
        public string tag;

        public UnityEvent _clickUIEvent = new UnityEvent();
        public UnityEvent _click2DEvent = new UnityEvent();
        public UnityEvent _click3DEvent = new UnityEvent();
        public UnityEvent _survoleUIEvent = new UnityEvent();
        public UnityEvent _survole2DEvent = new UnityEvent();
        public UnityEvent _survole3DEvent = new UnityEvent();

        public RaycastTag(string tag)
        {
            this.tag = tag;
        }
    }

    private void InstantiateOneTag(string tag) =>
        allTag.Add(tag, new RaycastTag(tag));

    private void HandleRaycast(Func<bool, bool> uiHandler, Func<bool, bool> raycast2D, Action<bool> raycast3D, bool clicked)
    {
        // Si pas d’UI ou que le raycast UI ne bloque pas le clic :
        if (UIRaycaster == null || !uiHandler(clicked))
        {
            if (!raycast2D(clicked))
                raycast3D(clicked);
        }
    }

    private bool UI(bool cliked)
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        UIRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            if (allTag.ContainsKey(results[0].gameObject.transform.tag))
            {
                if (cliked)
                    allTag[results[0].gameObject.tag]._clickUIEvent.Invoke();
                else
                    allTag[results[0].gameObject.tag]._survoleUIEvent.Invoke();
                return true;
            }
        }

        return false;
    }

    private bool TwoD(bool cliked)
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (allTag.ContainsKey(hit.transform.tag))
            {
                if (cliked)
                    allTag[hit.transform.tag]._click2DEvent.Invoke();
                else
                    allTag[hit.transform.tag]._survoleUIEvent.Invoke();
                return true;
            }
        }
        return false;
    }

    private void ThreeD(bool clicked)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Lancer un raycast 3D depuis la caméra
        if (Physics.Raycast(ray, out hit, 100f)) // 100f = distance max du rayon
        {
            // Vérifie si l’objet touché a un tag enregistré
            if (allTag.ContainsKey(hit.transform.tag))
            {
                if (clicked)
                    allTag[hit.transform.tag]._click3DEvent.Invoke();
                else
                    allTag[hit.transform.tag]._survole3DEvent.Invoke();
            }
        }
    }
}
