﻿using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.TITLE);
    }

    public void OnClickBtnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}