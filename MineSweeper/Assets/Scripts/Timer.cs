using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text time;

    private TimeSpan limitSecondsTS;

    private float totalSeconds;
    private float limitSeconds = 225f;

    private TimeSpan remainSecondsTS;
    private bool isStop;

    public void Awake()
    {
        limitSecondsTS = TimeSpan.FromSeconds(limitSeconds);
    }

    private void OnEnable()
    {
        totalSeconds = 0;
        isStop = false;
    }

    public void Update()
    {
        if (isStop) return;

        totalSeconds += Time.deltaTime;
        TimeSpan nowSeconds = TimeSpan.FromSeconds(totalSeconds);

        if (totalSeconds >= limitSeconds)
        {
            isStop = true;
            time.text = $"YOU FAILED";
            GameManager.Instance.inGame.SetBG_Color(0, limitSeconds);
            GameManager.Instance.inGame.GameOver();
            return;
        }

        remainSecondsTS = limitSecondsTS - nowSeconds;
        time.text = $"{remainSecondsTS.Minutes:00}:{remainSecondsTS.Seconds:00}:{remainSecondsTS.Milliseconds:000}";

        GameManager.Instance.inGame.SetBG_Color(remainSecondsTS.Minutes * 60 + remainSecondsTS.Seconds, limitSeconds);
    }
}
