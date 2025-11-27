using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoSingleton<Timer>
{
    public float _time;
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
            _time += Time.deltaTime;
            text.text = $"{(int)_time}s{Mathf.Floor((_time % 1f) * 100f):00}";
        }

        if (GameManager.I._replay && GameManager.I._state == EGameState.OVERWATCH)
            text.text = "";
    }

    private void SaveTimer()
    {
        if (_time < /*PlayerPrefs.GetFloat(MenuManager.I._indexMapPlayMode.ToString(), 99.99f) */MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode] && _time != 0f && EditorManager.WriteMap(EditorManager.I.currentMapData) == GV.GameSO._allMapList[MenuManager.I._indexMapPlayMode])
        {
            MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode] = _time;
            PlayerPrefs.SetFloat(MenuManager.I._indexMapPlayMode.ToString(), _time);
            PlayerPrefs.Save();
        }
    }

    private void REsetTimer()
    {
        _time = 0f;
        text.text = $"{(int)_time}s{Mathf.Floor((_time % 1f) * 100f):00}";
    }
}
