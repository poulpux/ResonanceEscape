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
    }

    [SerializeField] GameObject shapes;
    EEditorSelectionType selectionType;
    EMapType mapType;
    MapData currentMapData = new MapData();


    GameObject playerObject, winconditionObject;
    List<GameObject> allObject = new List<GameObject>();

    private void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._editorPlayer]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.PLAYER));
        RaycastManager_.I.allTag[GV.TagSO._editorWinCondition]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.WINCONDITION));
        RaycastManager_.I.allTag[GV.TagSO._editorWall]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.WALL));
        RaycastManager_.I.allTag[GV.TagSO._editorSemiWall]._click2DEvent.AddListener(() => SelectNewCase(EEditorSelectionType.SEMIWALL));

        //Faire une option pour maintenir
        InputSystem_.I._leftClick._event.AddListener(() => LeftClick());
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
        else if (selectionType == EEditorSelectionType.WALL || selectionType == EEditorSelectionType.SEMIWALL)
            VerifAllWallAndOther();
    }

    private void VerifAllWallAndOther()
    {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        mousePos.z = 10f; // profondeur depuis la caméra
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Décale ton repère pour viser le centre de chaque tile
        worldPos.x = Mathf.Floor(worldPos.x) + 0.5f;
        worldPos.y = Mathf.Floor(worldPos.y) + 0.5f;

        Vector2 pos = new Vector2(worldPos.x, worldPos.y);

        if(CanMovePlayer(Vector2Int.RoundToInt(pos), 0))
{
            if (VerifWalls(pos, 0.33f))
            {
                GameObject tile = null;
                if (selectionType == EEditorSelectionType.WALL)
                {
                    tile = Instantiate(GV.PrefabSO._wall);
                    tile.transform.position = new Vector3(pos.x, pos.y, 0f);
                    currentMapData._wallPosList.Add(pos);
                }
                allObject.Add(tile);
            }
        }

        //if (CanMovePlayer(pos,0))
        //{
        //    if (VerifWalls(pos + Vector2.right /** 0.5f + Vector2.up * 0.5f*/, 0.33f))
        //    {
        //        GameObject tile = null;
        //        if(selectionType == EEditorSelectionType.WALL)
        //        {
        //            tile = Instantiate(GV.PrefabSO._wall);
        //            tile.transform.position = new Vector3(pos.x /*+ 0.5f*/, pos.y /*+ 0.5f*/, 0f);
        //            currentMapData._wallPosList.Add(new Vector2(pos.x /*+ 0.5f*/, pos.y /*+ 0.5f*/));
        //        }
        //        else if(selectionType == EEditorSelectionType.SEMIWALL)
        //        {
        //            tile = Instantiate(GV.PrefabSO._semiWall);
        //            tile.transform.position = new Vector3(pos.x /*+ 0.5f*/, pos.y /*+ 0.5f*/, 0f);
        //            //Ajouter la direction plus tard
        //            currentMapData._semiWallPosList.Add((0,new Vector2(pos.x /*+ 0.5f*/, pos.y /*+ 0.5f*/)));
        //        }
        //        allObject.Add(tile);
        //    }
        //}
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

    private bool CanMovePlayer(Vector2 pos, int thinkness)
    {
        //limitMap
        if(!VerifLimitMap(pos, thinkness))
            return false;

        return true;
    }

    private bool VerifLimitMap(Vector2 pos, int thinkness)
    {
        if (currentMapData._mapTypeC1 == 0)
        {
            if (pos.x >= -10 + thinkness && pos.x <= 10 - thinkness && pos.y >= -5 + thinkness && pos.y <= 5 - thinkness)
                return true;
        }
        else if (currentMapData._mapTypeC1 == 1)
        {
            if (pos.x >= -5 + thinkness && pos.x <= 5 - thinkness && pos.y >= -10 + thinkness && pos.y <= 10 - thinkness)
                return true;
        }
        else if (currentMapData._mapTypeC1 == 2)
        {
            if (pos.x >= -10 + thinkness && pos.x <= 10 - thinkness && pos.y >= -10 + thinkness && pos.y <= 10 - thinkness)
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


        return true;
    }

    private void InstantiateAllMap()
    {
        GameObject map = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMap :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMap :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMap, shapes.transform);

        allObject.Add(map);

        InstantiatePlayer((Vector3)currentMapData._playerPosC2 + Vector3.forward * -0.3f);

        GameObject winCondition = Instantiate(GV.PrefabSO._winCondition, shapes.transform);
        winCondition.transform.position = (Vector3)currentMapData._winConditionC2 + Vector3.forward * -0.1f;
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
    }

    public static string WriteMap(MapData data)
    {
        string cat1 = data._mapTypeC1.ToString();

        string cat2 = $"{data._playerPosC2.x},{data._playerPosC2.y}";
        string cat3 = $"{data._winConditionC2.x},{data._winConditionC2.y}";

        string cat4 = string.Join("€", data._wallPosList.ConvertAll(v => $"{v.x},{v.y}"));

        string cat5 = string.Join("€", data._semiWallPosList.ConvertAll(e => $"{e.type},{e.pos.x},{e.pos.y}"));

        return $"{cat1}${cat2}${cat3}${cat4}${cat5}";
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

        return data;
    }

    private void InstantiatePlayer(Vector2 pos)
    {
        GameObject player = Instantiate(GV.PrefabSO._player, shapes.transform);
        player.transform.position = pos;
        playerObject = player;
        playerObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        allObject.Add(player);
    }
}
