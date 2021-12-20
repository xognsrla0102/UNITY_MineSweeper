using UnityEngine;

public enum BGM_Type
{
    TITLE,
    IN_GAME,
    BGM_NUM
}

public enum SFX_Type
{
    ON_CLICK,
    BREAK_BLOCK,
    BOMB,
    GAME_CLEAR
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] AudioClip[] bmpClips;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] AudioClip[] sfxClips;

    public void Awake()
    {
        Instance = this;
    }

    public void PlayBGM(BGM_Type bgmType)
    {
        bgmSource.clip = bmpClips[(int)bgmType];
        bgmSource.loop = bgmType != BGM_Type.IN_GAME;
        bgmSource.Play();
    }
}
