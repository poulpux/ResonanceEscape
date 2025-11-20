using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EditorManager : MonoSingleton<EditorManager>
{
    [System.Serializable]
    public class MapData
    {
        public int _mapTypeC1 = 0;
        public Vector2 _playerPosC2 = Vector2.zero;
        public Vector2 _winConditionC2 = Vector2.right*3f;
        public List<Vector2> _wallPosList = new List<Vector2>();
        public List<(int type, Vector2 pos)> _semiWallPosList = new List<(int, Vector2)>();

        public List<(int type, Vector2 pos)> _murBlobPosList = new List<(int, Vector2)>();
        public List<(int type, Vector2 pos)> _blobPosList = new List<(int, Vector2)>();
        public List<Vector2> _piksPosList = new List<Vector2>();
        public List<(int type, Vector2 pos)> _projectilePosList = new List<(int, Vector2)>();
        public List<Vector2> _inertieBoostPosList = new List<Vector2>();
        public List<Vector2> _blackHolePosList = new List<Vector2>();
    }

    public GameObject _shapes;
    EEditorSelectionType selectionType;
    EMapType mapType;
    public MapData currentMapData = new MapData();


    GameObject playerObject, winconditionObject, lines, feedback;
    public List<GameObject> _allObject = new List<GameObject>();
    int indexRotate = 0;
    int indexMapType;
    private void Start()
    {
        GameManager.I._enterInEditModeEvent.AddListener(() => 
        {
                F_ChangeMap(WriteMap(new MapData())); 
        });

        GameManager.I._enterInEditModePastEvent.AddListener(()=> F_ChangeMap(WriteMap(currentMapData)));     
        RaycastManager_.I.allTag[GV.TagSO._editorPlayer]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.PLAYER));
        RaycastManager_.I.allTag[GV.TagSO._editorWinCondition]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.WINCONDITION));
        RaycastManager_.I.allTag[GV.TagSO._editorWall]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.WALL));
        RaycastManager_.I.allTag[GV.TagSO._editorSemiWall]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.SEMIWALL));
        RaycastManager_.I.allTag[GV.TagSO._editorBloobWall]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.WALLBLOOB));
        RaycastManager_.I.allTag[GV.TagSO._editorBloob]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.BLOOB));
        RaycastManager_.I.allTag[GV.TagSO._editorSpike]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.SPIKS));
        RaycastManager_.I.allTag[GV.TagSO._editorProjectile]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.PROJECTILE));
        RaycastManager_.I.allTag[GV.TagSO._editorInertieBoost]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.INERTIEBOOST));
        RaycastManager_.I.allTag[GV.TagSO._editorStar]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.STAR));
        RaycastManager_.I.allTag[GV.TagSO._editorBlackHole]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.BLACKHOLE));
        RaycastManager_.I.allTag[GV.TagSO._editorBackToMenu]._click2DEvent.AddListener(() => F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]));
        RaycastManager_.I.allTag[GV.TagSO._editorSave]._click2DEvent.AddListener(() => SaveMap());
        RaycastManager_.I.allTag[GV.TagSO._menuPlay]._click2DEvent.AddListener(() => { if (lines != null) { lines.SetActive(false); SaveMap(true); _shapes.transform.localScale = Vector3.one * (currentMapData._mapTypeC1 == 0 ? 1f : 0.5f); } });
        RaycastManager_.I.allTag[GV.TagSO._editorMapType]._click2DEvent.AddListener(() => { indexMapType = currentMapData._mapTypeC1; currentMapData = new MapData(); indexMapType = indexMapType == 2 ? 0 : indexMapType += 1; currentMapData._mapTypeC1 = indexMapType; F_ChangeMap(WriteMap(currentMapData)); });
        RaycastManager_.I.allTag[GV.TagSO._editorClean]._click2DEvent.AddListener(() => { currentMapData = new MapData(); F_ChangeMap(WriteMap(currentMapData)); });
        MenuManager.I._changeLvEvent.AddListener(() => F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]));
        GameManager.I._winTheLevelEvent.AddListener(() => F_SetGoodPlayPlayer());

        //Faire une option pour maintenir
        //InputSystem_.I._leftClick._eventMaintain.AddListener(() => LeftClick());
        //InputSystem_.I._rightClick._eventMaintain.AddListener(() => Erase());
        InputSystem_.I._r._event.AddListener(() => { if (GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._state == EGameState.OVERWATCH) F_ResetMap(false);});
        InputSystem_.I._r._event.AddListener(() => { if (GameManager.I._state == EGameState.EDITOR) indexRotate = indexRotate == 3 ? 0 : indexRotate += 1; SelectNewCase(selectionType); });


        SelectNewCase(EEditorSelectionType.PLAYER);

        F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]);
    }

    private void FixedUpdate()
    {
        if (InputSystem_.I._leftClick._pressed && GameManager.I._state == EGameState.EDITOR)
            LeftClick();
        if (InputSystem_.I._rightClick._pressed && GameManager.I._state == EGameState.EDITOR)
            Erase();

        if(GameManager.I._state == EGameState.EDITOR)
            DrawFeedback();
        else
            feedback.transform.position = Vector2.one * -200f;
    }

    private void DrawFeedback()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // profondeur depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Décale ton repère pour viser le centre de chaque tile
        worldPos.x = Mathf.Floor(worldPos.x)/* + 0.5f*/;
        worldPos.y = Mathf.Floor(worldPos.y) /*+ 0.5f*/;

        Vector2 pos = new Vector2(worldPos.x, worldPos.y);
         
        if(!VerifLimitMap(pos, 1))
        {
            feedback.transform.position = Vector2.one * -200f;
            return;
        }

        if (selectionType == EEditorSelectionType.SEMIWALL)
        {
            feedback.transform.eulerAngles = Vector3.forward * 90 * indexRotate;
        }

        if (selectionType == EEditorSelectionType.PLAYER || selectionType == EEditorSelectionType.WINCONDITION)
        {
            Vector3 mousePos1 = UnityEngine.Input.mousePosition;
            mousePos1.z = 10f; // distance du plan que tu veux viser depuis la caméra
            Vector3 worldPos1 = Camera.main.ScreenToWorldPoint(mousePos1);
            Vector2Int posInt = new Vector2Int(Mathf.RoundToInt(worldPos1.x), Mathf.RoundToInt(worldPos1.y));
            feedback.transform.position = new Vector3(posInt.x, posInt.y, -0.4f + 1f * 0.1f);
        }
        else
            feedback.transform.position = new Vector3(pos.x, pos.y, 0f);

        if (selectionType == EEditorSelectionType.SEMIWALL)
        {
            if (indexRotate == 1)
                feedback.transform.position += Vector3.right;
            else if (indexRotate == 2)
                feedback.transform.position += Vector3.right + Vector3.up;
            else if (indexRotate == 3)
                feedback.transform.position += Vector3.up;
        }
    }

    public void F_SetGoodPlayPlayer()
    {
        playerObject.transform.localPosition = currentMapData._playerPosC2;
        Rigidbody2D playerRigidBody = playerObject.GetComponent<Rigidbody2D>();
        playerRigidBody.velocity = Vector3.zero;
    }

    public void F_ResetMap(bool player = true)
    {
        F_ChangeMap(WriteMap(currentMapData), player);
    }

    public void F_ChangeMap(string codeMap, bool player = true)
    {
        _shapes.transform.localScale = Vector3.one;
        _shapes.transform.position = Vector3.zero;
        if (_allObject.Count != 0)
        {
            for (int i = _allObject.Count - 1; i >= 0; i--)
            {
                if (/*!player && */_allObject[i] == playerObject)
                    continue;
                else
                {
                    Destroy(_allObject[i]);
                    _allObject.RemoveAt(i);
                }
            }
        }

        currentMapData = ReadMap(codeMap);
        InstantiateAllMap(player);
        if (currentMapData._mapTypeC1 != 0 && GameManager.I._state != EGameState.EDITOR && GameManager.I._state != EGameState.MENUEDITORMODE)
        {
            _shapes.transform.localScale = Vector3.one * 0.5f;
            _shapes.transform.position = Vector3.right * 0.8f;
        }
    }

    private void Erase()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // profondeur depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Décale ton repère pour viser le centre de chaque tile
        worldPos.x = Mathf.Floor(worldPos.x)/* + 0.5f*/;
        worldPos.y = Mathf.Floor(worldPos.y) /*+ 0.5f*/;

        Vector2 posInt = new Vector2(worldPos.x, worldPos.y);

        // Supprime un élément d'une liste sans casser le foreach
        void SafeRemove<T>(List<T> list, Predicate<T> match)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                    list.RemoveAt(i);
            }
        }

        void EraseFromList<T>(List<T> list, Func<T, Vector2> getPos)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Vector2 pos = getPos(list[i]);
                if (Vector2.Distance(pos, posInt) < 0.3f)
                {
                    // Trouve l’objet correspondant et détruit-le
                    for (int j = _allObject.Count - 1; j >= 0; j--)
                    {
                        var obj = _allObject[j];
                        if (obj == null) continue;

                        if ((Vector2)obj.transform.position == pos)
                        {
                            Destroy(obj);
                            _allObject.RemoveAt(j);
                            break;
                        }
                    }

                    list.RemoveAt(i);
                    return; // on peut arrêter ici
                }
            }
        }

        void EraseFromListSemiWall<T>(List<T> list, Func<T, Vector2> getPos, Func<T, int> getIndex)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Vector2 pos = getPos(list[i]);
                int index = getIndex(list[i]);

                Vector2 posByAdding = pos;
                if (index == 1)
                    posByAdding -= Vector2.right;
                else if(index == 2)
                    posByAdding -= Vector2.right+ Vector2.up;
                else if(index == 3)
                    posByAdding -= Vector2.up;

                if (Vector2.Distance(posByAdding, posInt) < 0.3f)
                {
                    // Trouve l’objet correspondant et détruit-le
                    for (int j = _allObject.Count - 1; j >= 0; j--)
                    {
                        var obj = _allObject[j];
                        if (obj == null) continue;

                        if ((Vector2)obj.transform.position == pos)
                        {
                            Destroy(obj);
                            _allObject.RemoveAt(j);
                            break;
                        }
                    }

                    list.RemoveAt(i);
                    return; // on peut arrêter ici
                }
            }
        }

        // Efface tous les types d’éléments
        EraseFromList(currentMapData._wallPosList, p => p);
        EraseFromListSemiWall(currentMapData._semiWallPosList, p => p.pos, p => p.type);
        EraseFromList(currentMapData._piksPosList, p => p);
        EraseFromList(currentMapData._blobPosList, p => p.pos);
        EraseFromList(currentMapData._murBlobPosList, p => p.pos);
        EraseFromList(currentMapData._projectilePosList, p => p.pos);
        EraseFromList(currentMapData._blackHolePosList, p => p);
        EraseFromList(currentMapData._inertieBoostPosList, p => p);
    }

    private void SelectNewCase(EEditorSelectionType selectionType)
    {
        this.selectionType = selectionType;
        if(feedback != null)
            Destroy(feedback);

        if(selectionType == EEditorSelectionType.PLAYER)
            feedback = Instantiate(GV.PrefabSO._previsuPlayer, _shapes.transform);
        else if (selectionType == EEditorSelectionType.WINCONDITION)
            feedback = Instantiate(GV.PrefabSO._previsuWinCondition, _shapes.transform);
        else if (selectionType == EEditorSelectionType.WALL)
            feedback = Instantiate(GV.PrefabSO._wall, _shapes.transform);
        else if (selectionType == EEditorSelectionType.SEMIWALL)
            feedback = Instantiate(GV.PrefabSO._semiWall, _shapes.transform);
        else if (selectionType == EEditorSelectionType.WALLBLOOB)
            feedback = Instantiate(indexRotate % 2 == 0 ? GV.PrefabSO._murBloobPlein : GV.PrefabSO._murBloobVide, _shapes.transform);
        else if (selectionType == EEditorSelectionType.BLOOB)
            feedback = Instantiate(indexRotate % 2 == 0 ? GV.PrefabSO._bloobPlein : GV.PrefabSO._bloobVide, _shapes.transform);
        else if (selectionType == EEditorSelectionType.SPIKS)
            feedback = Instantiate(GV.PrefabSO._piks, _shapes.transform);
        else if (selectionType == EEditorSelectionType.PROJECTILE)
            feedback = Instantiate(GV.PrefabSO._projectile, _shapes.transform);
        else if (selectionType == EEditorSelectionType.INERTIEBOOST)
            feedback = Instantiate(GV.PrefabSO._inertieBoost, _shapes.transform);
        else if (selectionType == EEditorSelectionType.BLACKHOLE)
            feedback = Instantiate(GV.PrefabSO._blackHole, _shapes.transform);
    }

    private void SaveMap(bool playmode = false)
    {
        GUIUtility.systemCopyBuffer = WriteMap(currentMapData);
        if (currentMapData._mapTypeC1 == 0 && playmode)
            return;
        GameManager.I._saveEvent.Invoke();
    }

    private void LeftClick()
    {
        if (selectionType == EEditorSelectionType.PLAYER)
            VerifAllPlayerAndWin(1, ref playerObject);
        else if (selectionType == EEditorSelectionType.WINCONDITION)
            VerifAllPlayerAndWin(2, ref winconditionObject);
        else if (selectionType == EEditorSelectionType.WALL 
            || selectionType == EEditorSelectionType.SEMIWALL
            || selectionType == EEditorSelectionType.WALLBLOOB
            || selectionType == EEditorSelectionType.BLOOB
            || selectionType == EEditorSelectionType.SPIKS
            || selectionType == EEditorSelectionType.PROJECTILE
            || selectionType == EEditorSelectionType.INERTIEBOOST
            || selectionType == EEditorSelectionType.STAR
            || selectionType == EEditorSelectionType.BLACKHOLE)
            VerifAllWallAndOther();
    }

    private void VerifAllWallAndOther()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // profondeur depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Décale ton repère pour viser le centre de chaque tile
        worldPos.x = Mathf.Floor(worldPos.x)/* + 0.5f*/;
        worldPos.y = Mathf.Floor(worldPos.y) /*+ 0.5f*/;

        Vector2 pos = new Vector2(worldPos.x, worldPos.y);

        if(CanMovePlayer(Vector2Int.RoundToInt(pos), 0,true))
{
            if (VerifWalls(pos, 0.33f) && VerifPlayerAndWincondition(pos, 0.33f))
            {
                GameObject tile = null;
                if (selectionType == EEditorSelectionType.WALL)
                {
                    tile = Instantiate(GV.PrefabSO._wall, _shapes.transform);
                    currentMapData._wallPosList.Add(pos);
                }
                else if(selectionType == EEditorSelectionType.SEMIWALL)
                {
                    tile = Instantiate(GV.PrefabSO._semiWall, _shapes.transform);
                    tile.transform.eulerAngles = Vector3.forward * 90 * indexRotate;
                }
                else if(selectionType == EEditorSelectionType.WALLBLOOB)
                {
                    tile = Instantiate(indexRotate % 2 == 0 ? GV.PrefabSO._murBloobPlein : GV.PrefabSO._murBloobVide, _shapes.transform);
                    currentMapData._murBlobPosList.Add((indexRotate, pos));
                }
                else if(selectionType == EEditorSelectionType.BLOOB)
                {
                    tile = Instantiate(indexRotate % 2 == 0 ? GV.PrefabSO._bloobPlein : GV.PrefabSO._bloobVide, _shapes.transform);
                    currentMapData._blobPosList.Add((indexRotate, pos));
                }
                else if(selectionType == EEditorSelectionType.SPIKS)
                {
                    tile = Instantiate(GV.PrefabSO._piks, _shapes.transform);
                    currentMapData._piksPosList.Add(pos);
                }
                else if(selectionType == EEditorSelectionType.PROJECTILE)
                {
                    tile = Instantiate(GV.PrefabSO._projectile, _shapes.transform);
                    currentMapData._projectilePosList.Add((indexRotate, pos));
                }
                else if(selectionType == EEditorSelectionType.INERTIEBOOST)
                {
                    tile = Instantiate(GV.PrefabSO._inertieBoost, _shapes.transform);
                    currentMapData._inertieBoostPosList.Add(pos);
                }
                else if(selectionType == EEditorSelectionType.BLACKHOLE)
                {
                    tile = Instantiate(GV.PrefabSO._blackHole, _shapes.transform);
                    currentMapData._blackHolePosList.Add(pos);
                }

                tile.transform.position = new Vector3(pos.x, pos.y, 0f);
                if (selectionType == EEditorSelectionType.SEMIWALL)
                {
                    if (indexRotate == 1)
                        tile.transform.position += Vector3.right;
                    else if(indexRotate == 2)
                        tile.transform.position += Vector3.right + Vector3.up;
                    else if(indexRotate == 3)
                        tile.transform.position += Vector3.up;

                    currentMapData._semiWallPosList.Add((indexRotate, (Vector2)tile.transform.position));
                }
                _allObject.Add(tile);
            }
        }
    }

    private void VerifAllPlayerAndWin(int thikness, ref GameObject objectToMove)
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // distance du plan que tu veux viser depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2Int posInt = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
        if (CanMovePlayer(posInt, thikness))
        {
            //Verif les murs maintenant !
            if (VerifWalls(posInt, thikness))
            {
                objectToMove.transform.position = new Vector3(posInt.x, posInt.y, -0.4f + thikness * 0.1f);

                if (thikness == 1)
                    currentMapData._playerPosC2 = new Vector2(posInt.x, posInt.y);
                else 
                    currentMapData._winConditionC2 = new Vector2(posInt.x, posInt.y);

            }
        }
    }

    private bool CanMovePlayer(Vector2 pos, int thinkness, bool cube = false)
    {
        //limitMap
        if(!VerifLimitMap(pos, thinkness, cube))
            return false;

        return true;
    }

    private bool VerifLimitMap(Vector2 pos, int thinkness, bool cube = false)
    {
        if (currentMapData._mapTypeC1 == 0)
        {
            if (pos.x >= -10 + thinkness && pos.x <= 10 - thinkness - (cube ? 1 : 0) && pos.y >= -5 + thinkness && pos.y <= 5 - thinkness-(cube ? 1 : 0))
                return true;
        }
        else if (currentMapData._mapTypeC1 == 1)
        {
            if (pos.x >= -5 + thinkness && pos.x <= 5 - thinkness - (cube ? 1 : 0) && pos.y >= -10 + thinkness && pos.y <= 10 - thinkness - (cube ? 1 : 0))
                return true;
        }
        else if (currentMapData._mapTypeC1 == 2)
        {
            if (pos.x >= -10 + thinkness && pos.x <= 10 - thinkness - (cube ? 1 : 0) && pos.y >= -10 + thinkness && pos.y <= 10 - thinkness - (cube ? 1 : 0))
                return true;
        }

        return false;
    }

    private bool VerifWalls(Vector2 pos, float thinkness)
    {
        foreach (var item in currentMapData._wallPosList)
        {
            if (Vector2.Distance(item, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._semiWallPosList)
        {
            Vector2 posByAdding = pos;
            if (item.type == 1)
                posByAdding -= Vector2.right;
            else if (item.type == 2)
                posByAdding -= Vector2.right + Vector2.up;
            else if (item.type == 3)
                posByAdding -= Vector2.up;
            if (Vector2.Distance(item.pos, posByAdding) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._piksPosList)
        {
            if (Vector2.Distance(item, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._blobPosList)
        {
            if (Vector2.Distance(item.pos, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._murBlobPosList)
        {
            if (Vector2.Distance(item.pos, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._projectilePosList)
        {
            if (Vector2.Distance(item.pos, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._blackHolePosList)
        {
            if (Vector2.Distance(item, pos) < (float)thinkness)
                return false;
        }
        foreach (var item in currentMapData._inertieBoostPosList)
        {
            if (Vector2.Distance(item, pos) < (float)thinkness)
                return false;
        }


        //Continuer ici 


        return true;
    }

    private bool VerifPlayerAndWincondition(Vector2 pos, float thinkness)
    {
        if (Vector2.Distance(playerObject.transform.position - Vector3.right * 0.5f - Vector3.up * 0.5f, pos) < (float)thinkness + 0.66f)
            return false;
        
        if (Vector2.Distance(winconditionObject.transform.position-Vector3.right*0.5f-Vector3.up*0.5f, pos) < (float)thinkness + 0.99f)
            return false;

        return true;
    }

    private void InstantiateAllMap(bool player)
    {
        GameObject map = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMap :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMap :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMap, _shapes.transform);

        _allObject.Add(map);

        if(GameManager.I._state == EGameState.EDITOR)
        {
            GameObject lines = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMapGrille :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMapGrille :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMapGrille, _shapes.transform);

            this.lines = lines;
            _allObject.Add(lines);
        }

        if (player)
        {
            InstantiatePlayer((Vector3)currentMapData._playerPosC2 + Vector3.forward * -0.4f);
        }

        GameObject winCondition = Instantiate(GV.PrefabSO._winCondition, _shapes.transform);
        winCondition.transform.position = (Vector3)currentMapData._winConditionC2 + Vector3.forward * -0.3f;
        winconditionObject = winCondition;
        _allObject.Add(winCondition);

        foreach (var item in currentMapData._wallPosList)
        {
            GameObject wall = Instantiate(GV.PrefabSO._wall, _shapes.transform);
            wall.transform.position = item;
            _allObject.Add(wall);
        }

        foreach (var item in currentMapData._semiWallPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._semiWall, _shapes.transform);
            semiWall.transform.position = item.pos;
            //if (item.type == 1)
            //    semiWall.transform.position += Vector3.right*0.5f;
            //else if (item.type == 2)
            //    semiWall.transform.position += Vector3.right*0.5f + Vector3.up*0.5f;
            //else if (item.type == 3)
            //    semiWall.transform.position += Vector3.up*0.5f;

            semiWall.transform.eulerAngles = Vector3.forward * item.type * 90;
            _allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._murBlobPosList)
        {
            GameObject semiWall = Instantiate(item.type % 2 == 0 ? GV.PrefabSO._murBloobPlein : GV.PrefabSO._murBloobVide, _shapes.transform);
            semiWall.transform.position = item.pos;
            _allObject.Add(semiWall);
        }
        
        foreach (var item in currentMapData._blobPosList)
        {
            GameObject semiWall = Instantiate(item.type % 2 == 0 ? GV.PrefabSO._bloobPlein : GV.PrefabSO._bloobVide, _shapes.transform);
            semiWall.transform.position = item.pos;
            _allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._piksPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._piks, _shapes.transform);
            semiWall.transform.position = item;
            _allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._projectilePosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._projectile, _shapes.transform);
            semiWall.transform.position = item.pos;
            semiWall.transform.eulerAngles = Vector3.forward * item.type * 45f;
            _allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._inertieBoostPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._star, _shapes.transform);
            semiWall.transform.position = item;
            _allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._blackHolePosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._blackHole, _shapes.transform);
            semiWall.transform.position = item;
            _allObject.Add(semiWall);
        }
    }

    public static string WriteMap(MapData data)
    {
        string cat1 = data._mapTypeC1.ToString();

        string cat2 = $"{data._playerPosC2.x},{data._playerPosC2.y}";
        string cat3 = $"{data._winConditionC2.x},{data._winConditionC2.y}";

        string cat4 = string.Join("€", data._wallPosList.ConvertAll(v => $"{v.x},{v.y}"));

        string cat5 = string.Join("€", data._semiWallPosList.ConvertAll(e => $"{e.type},{e.pos.x},{e.pos.y}"));
        string cat6 = string.Join("€", data._murBlobPosList.ConvertAll(e => $"{e.type},{e.pos.x},{e.pos.y}"));
        string cat7 = string.Join("€", data._blobPosList.ConvertAll(e => $"{e.type},{e.pos.x},{e.pos.y}"));

        string cat8 = string.Join("€", data._piksPosList.ConvertAll(v => $"{v.x},{v.y}"));

        string cat9 = string.Join("€", data._projectilePosList.ConvertAll(e => $"{e.type},{e.pos.x},{e.pos.y}"));

        string cat10 = string.Join("€", data._inertieBoostPosList.ConvertAll(v => $"{v.x},{v.y}"));
        string cat11 = string.Join("€", data._blackHolePosList.ConvertAll(v => $"{v.x},{v.y}"));

        return $"{cat1}${cat2}${cat3}${cat4}${cat5}${cat6}${cat7}${cat8}${cat9}${cat10}${cat11}";
    }

    public static MapData ReadMap(string mapString)
    {
        try
        {
            string[] parts = mapString.Split('$');
            MapData data = new MapData();

            if (parts.Length > 0) data._mapTypeC1 = int.Parse(parts[0]);
            if (parts.Length > 1)
            {
                string[] v = parts[1].Split(',');
                data._playerPosC2 = new Vector2(float.Parse(v[0]), float.Parse(v[1]));
            }
            if (parts.Length > 2)
            {
                string[] v = parts[2].Split(',');
                data._winConditionC2 = new Vector2(float.Parse(v[0]), float.Parse(v[1]));
            }
            if (parts.Length > 3)
            {
                string[] entries = parts[3].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 2)
                        data._wallPosList.Add(new Vector2(float.Parse(v[0]), float.Parse(v[1])));
                }
            }
            if (parts.Length > 4)
            {
                string[] entries = parts[4].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 3)
                        data._semiWallPosList.Add((int.Parse(v[0]), new Vector2(float.Parse(v[1]), float.Parse(v[2]))));
                }
            }
            if (parts.Length > 5)
            {
                string[] entries = parts[5].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 3)
                        data._murBlobPosList.Add((int.Parse(v[0]), new Vector2(float.Parse(v[1]), float.Parse(v[2]))));
                }
            }
            if (parts.Length > 6)
            {
                string[] entries = parts[6].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 3)
                        data._blobPosList.Add((int.Parse(v[0]), new Vector2(float.Parse(v[1]), float.Parse(v[2]))));
                }
            }
            if (parts.Length > 7)
            {
                string[] entries = parts[7].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 2)
                        data._piksPosList.Add(new Vector2(float.Parse(v[0]), float.Parse(v[1])));
                }
            }
            if (parts.Length > 8)
            {
                string[] entries = parts[8].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 3)
                        data._projectilePosList.Add((int.Parse(v[0]), new Vector2(float.Parse(v[1]), float.Parse(v[2]))));
                }
            }
            if (parts.Length > 9)
            {
                string[] entries = parts[9].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 2)
                        data._inertieBoostPosList.Add(new Vector2(float.Parse(v[0]), float.Parse(v[1])));
                }
            }
            if (parts.Length > 9)
            {
                string[] entries = parts[10].Split('€');
                foreach (string entry in entries)
                {
                    string[] v = entry.Split(',');
                    if (v.Length == 2)
                        data._blackHolePosList.Add(new Vector2(float.Parse(v[0]), float.Parse(v[1])));
                }
            }

            return data;
        }
        catch 
        {
            // 🔴 Invoquer un event d’erreur
            GameManager.I._pastErrorEvent?.Invoke();
            return null;
        }
    }

    private void InstantiatePlayer(Vector3 pos)
    {
        GameObject player = null;
        if (playerObject == null)
        {
            player = Instantiate(GV.PrefabSO._player, _shapes.transform);
            playerObject = player;  
        }

        playerObject.transform.position = pos;
        PlayerMovement.I._rigidBody.bodyType = RigidbodyType2D.Kinematic;
        _allObject.Add(player);
    }
}
