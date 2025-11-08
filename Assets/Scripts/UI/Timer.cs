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
    }

    private void SaveTimer()
    {
        if (time < PlayerPrefs.GetFloat(MenuManager.I._indexMapPlayMode.ToString(), 99.99f))
        {
            PlayerPrefs.SetFloat(MenuManager.I._indexMapPlayMode.ToString(), time);
        }
    }

    private void REsetTimer()
    {
        time = 0f;
        text.text = $"{(int)time}s{Mathf.Floor((time % 1f) * 100f):00}";
    }
}
