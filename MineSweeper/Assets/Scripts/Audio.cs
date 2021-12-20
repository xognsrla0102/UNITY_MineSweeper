using UnityEngine;

[System.Serializable]
public class Audio
{
    [HideInInspector] public AudioSource source;

    public BGM_Type bgmType;
    public SFX_Type sfxType;

    public AudioClip clip;
    [Range(0, 1)] public float volume;
    public bool isLoop;
}
