using System.Collections;
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

    List<GameObject> allObject = new List<GameObject>();

    private void Start()
    {
        InstantiateAllMap();
    }

    private void InstantiateAllMap()
    {
        GameObject map = Instantiate(currentMapData._mapTypeC1 == 0 ? GV.PrefabSO._largeMap :
            currentMapData._mapTypeC1 == 1 ? GV.PrefabSO._longMap :
            /*currentMapData._mapTypeC1 == 2 ?*/ GV.PrefabSO._bothMap, shapes.transform);

        allObject.Add(map);

        GameObject player = Instantiate(GV.PrefabSO._player, shapes.transform);
        player.transform.position = (Vector3)currentMapData._playerPosC2 + Vector3.forward *-0.3f;

        allObject.Add(player);

        GameObject winCondition = Instantiate(GV.PrefabSO._winCondition, shapes.transform);
        winCondition.transform.position = (Vector3)currentMapData._winConditionC2 + Vector3.forward * -0.1f;
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
}
