using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : MonoBehaviour
{
    [SerializeField] private Slider BGM_Slider;
    [SerializeField] private Slider SFX_Slider;

    public void OnEnable()
    {
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Volume", 1f);
    }

    public void OnDisable()
    {
        PlayerPrefs.SetFloat("BGM_Volume", BGM_Slider.value);
        PlayerPrefs.SetFloat("SFX_Volume", SFX_Slider.value);
    }

    public void OnClickBtnBack()
    {
        gameObject.SetActive(false);
    }

    public void OnChangeValueBGMVolume()
    {
        SoundManager.Instance.SetBGMVolume(BGM_Slider.value);
    }

    public void OnChangeValueSFXVolume()
    {
        SoundManager.Instance.SetSFXVolume(SFX_Slider.value);
    }
}
