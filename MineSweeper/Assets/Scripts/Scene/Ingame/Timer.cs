﻿using System.Diagnostics;
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

    private float RemainTime
    {
        get
        {
            // offset만큼 시간 느리게 적용
            float applyOffsetTime = ingameTimer.ElapsedMilliseconds / timeOffset;
            // 초 단위로 변경
            return (LIMIT_MILI_SECONDS - applyOffsetTime) / 1000f;
        }
    }
    private readonly Stopwatch ingameTimer = new();
    private readonly Stopwatch secTimer = new();

    public bool IsBlack
    {
        get => isBlack;
        set
        {
            if (isBlack == value) return;

            isBlack = value;
            GetComponent<Outline>().enabled = isBlack;
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

        ingameTimer.Start();

        StartCoroutine(CheckVelTime());
    }

    public void OnDisable()
    {
        ingameTimer.Stop();
        if (secTimer.IsRunning) secTimer.Stop();
    }

    public void Update()
    {
        if (isStop) return;
        timeText.text = $"{RemainTime:0.000}";
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

                float remainTime = RemainTime;

                // 10초 지난 횟수 만큼 속도 가중치를 더함[0.25 배]
                timeOffset = 2.25f - (int)(10 - remainTime / 10) * 0.0025f;

                if (remainTime <= 80)
                    IsBlack = true;

                if (remainTime <= 0)
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
