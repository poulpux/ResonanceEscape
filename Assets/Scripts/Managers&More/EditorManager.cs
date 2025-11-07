using System.Collections.Generic;
using UnityEngine;

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
        public List<Vector2> _starPosList = new List<Vector2>();
        public List<Vector2> _blackHolePosList = new List<Vector2>();
    }

    [SerializeField] GameObject shapes;
    EEditorSelectionType selectionType;
    EMapType mapType;
    MapData currentMapData = new MapData();


    GameObject playerObject, winconditionObject;
    List<GameObject> allObject = new List<GameObject>();

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
        RaycastManager_.I.allTag[GV.TagSO._editorSave]._click2DEvent.AddListener(() => GUIUtility.systemCopyBuffer = WriteMap(currentMapData));
        MenuManager.I._changeLvEvent.AddListener(() => F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]));
        GameManager.I._winTheLevelEvent.AddListener(() => F_SetGoodPlayPlayer());
        //Faire une option pour maintenir
        InputSystem_.I._leftClick._event.AddListener(() => LeftClick());

        F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]);
    }



    public void F_SetGoodPlayPlayer()
    {
        playerObject.transform.position = currentMapData._playerPosC2;
        Rigidbody2D playerRigidBody = playerObject.GetComponent<Rigidbody2D>();
        playerRigidBody.velocity = Vector3.zero;
    }

    public void F_ResetMap()
    {
        F_ChangeMap(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]);
    }

    public void F_ChangeMap(string codeMap)
    {
        print("changeMaP");
        if(allObject.Count != 0)
        {
            for (int i = allObject.Count-1; i >= 0; i--)
                Destroy(allObject[i]); 
        }

        currentMapData = ReadMap(codeMap);
        InstantiateAllMap();
    }

    private void SelectNewCase(EEditorSelectionType selectionType)
    {
        //print("seld")
        this.selectionType = selectionType;
    }

    private void LeftClick()
    {
        if (GameManager.I._state != EGameState.EDITOR) return;

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
                    tile = Instantiate(GV.PrefabSO._wall);
                    currentMapData._wallPosList.Add(pos);
                }
                else if(selectionType == EEditorSelectionType.SEMIWALL)
                {
                    tile = Instantiate(GV.PrefabSO._semiWall);
                    currentMapData._semiWallPosList.Add((0,pos));
                }
                tile.transform.position = new Vector3(pos.x, pos.y, 0f);
                allObject.Add(tile);
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
            if (Vector2.Distance(item.pos, pos) < (float)thinkness)
                return false;
        }

        //Continuer ici 


        return true;
    }

    private bool VerifPlayerAndWincondition(Vector2 pos, float thinkness)
    {
        if (Vector2.Distance(playerObject.transform.position, pos) < (float)thinkness + 0.66)
            return false;
        
        if (Vector2.Distance(winconditionObject.transform.position, pos) < (float)thinkness + 1.66f)
            return false;

        return true;
    }

    private void InstantiateAllMap()
    {
        GameObject map = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMap :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMap :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMap, shapes.transform);

        allObject.Add(map);

        if(GameManager.I._state == EGameState.MENUEDITORMODE)
        {
            GameObject lines = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMapGrille :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMapGrille :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMapGrille, shapes.transform);

            allObject.Add(lines);
        }


        InstantiatePlayer((Vector3)currentMapData._playerPosC2 + Vector3.forward * -0.4f);

        GameObject winCondition = Instantiate(GV.PrefabSO._winCondition, shapes.transform);
        winCondition.transform.position = (Vector3)currentMapData._winConditionC2 + Vector3.forward * -0.3f;
        winconditionObject = winCondition;
        allObject.Add(winCondition);

        foreach (var item in currentMapData._wallPosList)
        {
            GameObject wall = Instantiate(GV.PrefabSO._wall, shapes.transform);
            wall.transform.position = item;
            allObject.Add(wall);
        }

        foreach (var item in currentMapData._semiWallPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._semiWall, shapes.transform);
            semiWall.transform.position = item.pos;
            semiWall.transform.eulerAngles = Vector3.forward * item.type * 45f;
            allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._murBlobPosList)
        {
            GameObject semiWall = Instantiate(item.type == 0 ? GV.PrefabSO._murBloobPlein : GV.PrefabSO._murBloobVide, shapes.transform);
            semiWall.transform.position = item.pos;
            allObject.Add(semiWall);
        }
        
        foreach (var item in currentMapData._blobPosList)
        {
            GameObject semiWall = Instantiate(item.type == 0 ? GV.PrefabSO._bloobPlein : GV.PrefabSO._bloobVide, shapes.transform);
            semiWall.transform.position = item.pos;
            allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._piksPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._piks, shapes.transform);
            semiWall.transform.position = item;
            allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._projectilePosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._projectile, shapes.transform);
            semiWall.transform.position = item.pos;
            semiWall.transform.eulerAngles = Vector3.forward * item.type * 45f;
            allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._starPosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._star, shapes.transform);
            semiWall.transform.position = item;
            allObject.Add(semiWall);
        }

        foreach (var item in currentMapData._blackHolePosList)
        {
            GameObject semiWall = Instantiate(GV.PrefabSO._blackHole, shapes.transform);
            semiWall.transform.position = item;
            allObject.Add(semiWall);
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

        string cat10 = string.Join("€", data._starPosList.ConvertAll(v => $"{v.x},{v.y}"));
        string cat11 = string.Join("€", data._blackHolePosList.ConvertAll(v => $"{v.x},{v.y}"));

        return $"{cat1}${cat2}${cat3}${cat4}${cat5}${cat6}${cat7}${cat8}${cat9}${cat10}${cat11}";
    }

    public static MapData ReadMap(string mapString)
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
                    data._starPosList.Add(new Vector2(float.Parse(v[0]), float.Parse(v[1])));
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

    private void InstantiatePlayer(Vector3 pos)
    {
        GameObject player = Instantiate(GV.PrefabSO._player, shapes.transform);
        player.transform.position = pos;
        playerObject = player;
        playerObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        allObject.Add(player);
    }
}
