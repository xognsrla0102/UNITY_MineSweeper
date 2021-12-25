using UnityEngine;
using System;
using System.Collections.Generic;

public enum BGM_Type
{
    NONE,
    TITLE,
    IN_GAME,
    GAME_CLEAR,
    GAME_FAIL,
    BGM_NUM
}

public enum SFX_Type
{
    NONE,
    ON_CLICK,
    BREAK_BLOCK_1,
    BREAK_BLOCK_2,
    BREAK_BLOCK_3,
    BREAK_BLOCK_4,
    BOMB,
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    public List<Audio> bgms;
    public List<Audio> sfxs;

    private float bgmVolume;
    private float sfxVolume;

    public void Awake()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_Volume", 1f);

        foreach (var audio in bgms)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;

            audio.source.volume = bgmVolume;
            audio.source.loop = audio.isLoop;
        }

        foreach (var audio in sfxs)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;

            audio.source.volume = sfxVolume;
            audio.source.loop = audio.isLoop;
        }
    }

    public void PlayBGM(BGM_Type bgmType)
    {
        // 오디오 중 BGM 중에서 실행 중인 오디오를 중지 후
        Audio bgm = bgms.Find(audio => audio.source.isPlaying);
        if (bgm != null) bgm.source.Stop();

        // BGM을 찾아서 실행
        bgm = bgms.Find(audio => audio.bgmType == bgmType);
        if (bgm == null) return;

        bgm.source.Play();
    }

    public void PlaySFX(SFX_Type sfxType)
    {
        Audio sfx = sfxs.Find(audio => audio.sfxType == sfxType);
        if (sfx == null) return;
        sfx.source.Play();
    }

    public void StopBGM()
    {
        Audio bgm = bgms.Find(audio => audio.source.isPlaying);
        if (bgm != null) bgm.source.Stop();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;

        foreach (var audio in bgms)
            audio.source.volume = bgmVolume;            
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        foreach (var audio in sfxs)
            audio.source.volume = sfxVolume;
    }
}
