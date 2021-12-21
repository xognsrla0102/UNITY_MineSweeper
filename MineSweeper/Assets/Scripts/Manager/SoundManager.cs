using UnityEngine;
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
    public static SoundManager Instance;

    public List<Audio> audios;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        foreach (var audio in audios)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;

            audio.source.volume = audio.volume;
            audio.source.loop = audio.isLoop;
        }
    }

    public void PlayBGM(BGM_Type bgmType)
    {
        Audio bgm = audios.Find(audio => audio.source.isPlaying);
        if (bgm != null) bgm.source.Stop();

        bgm = audios.Find(audio => audio.bgmType == bgmType);
        if (bgm == null) return;

        bgm.source.Play();
    }

    public void PlaySFX(SFX_Type sfxType)
    {
        Audio sfx = audios.Find(audio => audio.sfxType == sfxType);
        if (sfx == null) return;
        sfx.source.Play();
    }
}
