using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private const int LIMIT_MILI_SECONDS = 100_000;

    private Text timeText;

    private float timeOffset;
    private bool isStop;
    private bool isBlack;

    // offset만큼 시간 느리게 적용
    public float TotalTime => ingameTimer.ElapsedMilliseconds / timeOffset;

    private float RemainTimeSec => (LIMIT_MILI_SECONDS - TotalTime) / 1000f;

    private readonly Stopwatch ingameTimer = new Stopwatch();
    private readonly Stopwatch secTimer = new Stopwatch();

    public bool IsBlack
    {
        get => isBlack;
        set
        {
            GetComponent<Outline>().enabled = value;
            if (isBlack == value) return;

            isBlack = value;
            GameManager.Instance.inGame.SetBG_Color(isBlack ? Color.black : Color.white);
        }
    }

    public void Awake()
    {
        timeText = GetComponent<Text>();
    }

    public void OnEnable()
    {
        // 브금에 맞추기 때문에 현실 세계의 1초가 아님
        timeOffset = 2.25f;

        isStop = false;
        IsBlack = false;

        ingameTimer.Restart();

        StartCoroutine(CheckVelTime());
    }

    public void OnDisable()
    {
        StopTimer();
    }

    public void StopTimer()
    {
        if (ingameTimer.IsRunning) ingameTimer.Stop();
        if (secTimer.IsRunning) secTimer.Stop();
    }

    public void Update()
    {
        if (isStop) return;
        timeText.text = $"{RemainTimeSec:0.000}";
    }

    private IEnumerator CheckVelTime()
    {
        secTimer.Start();

        while (true)
        {
            // 인게임 시간의 1초 마다 여기로 들어옴
            if(secTimer.ElapsedMilliseconds / timeOffset >= 1_000)
            {
                secTimer.Restart();

                float remainTime = RemainTimeSec;

                // 10초 지난 횟수 만큼 속도 가중치를 더함[0.25 배]
                timeOffset = 2.25f - (int)(10 - remainTime / 10) * 0.0025f;

                if (remainTime <= 81)
                    IsBlack = true;

                if (remainTime < 1)
                {
                    isStop = true;
                    timeText.text = $"YOU FAILED";
                    GameManager.Instance.inGame.GameOver();
                    yield break;
                }
            }
            yield return null;
        }
    }
}
