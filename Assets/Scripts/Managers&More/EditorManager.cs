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

    EEditorSelectionType selectionType;
    EMapType mapType;
    MapData currentMapData = new MapData();
    public string test;
    // Start is called before the first frame update
    void Start()
    {
        print(WriteMap(currentMapData));
        test = WriteMap(currentMapData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string WriteMap(MapData data)
    {
        string cat1 = data._mapTypeC1.ToString();

        string cat2 = "µ{data._playerPosC2.x},{data._playerPosC2.y}";
        string cat3 = "µ{data._winConditionC2.x},{data._winConditionC2.y}";

        string cat4 = string.Join("€", data._wallPosList.ConvertAll(v => "µ{v.x},{v.y}"));

        string cat5 = string.Join("€", data._semiWallPosList.ConvertAll(e => "µ{e.type},{e.pos.x},{e.pos.y}"));

        return "µ{cat1}µ{cat2}µ{cat3}µ{cat4}µ{cat5}";
    }

    public static MapData ReadMap(string mapString)
    {
        string[] parts = mapString.Split('µ');
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
