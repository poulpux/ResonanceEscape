using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float time;
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        InputSystem_.I._r._event.AddListener(() => REsetTimer());
        GameManager.I._overwatchEvent.AddListener(()=> REsetTimer());
        GameManager.I._winTheLevelEvent.AddListener(()=> SaveTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I._state == EGameState.ACT)
        {
            time += Time.deltaTime;
            text.text = $"{(int)time}s{Mathf.Floor((time % 1f) * 100f):00}";
        }

        if (GameManager.I._replay && GameManager.I._state == EGameState.OVERWATCH)
            text.text = "";
    }

    private void SaveTimer()
    {
        print(time < MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode]);
        print(time != 0f);
        print(EditorManager.WriteMap(EditorManager.I.currentMapData) == GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]);
        print(EditorManager.WriteMap(EditorManager.I.currentMapData));
        print(GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode]);
        if (time < /*PlayerPrefs.GetFloat(MenuManager.I._indexMapPlayMode.ToString(), 99.99f) */MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode] && time != 0f && EditorManager.WriteMap(EditorManager.I.currentMapData) == GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode])
        {
            print("save timer");
            MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode] = time;
            PlayerPrefs.SetFloat(MenuManager.I._indexMapPlayMode.ToString(), time);
            PlayerPrefs.Save();
        }
    }

    private void REsetTimer()
    {
        time = 0f;
        text.text = $"{(int)time}s{Mathf.Floor((time % 1f) * 100f):00}";
    }
}
