using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text bestRecordText;
    [SerializeField] private GameObject optionPopup;
    [SerializeField] private GameObject dimImg;

    public void OnEnable()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.TITLE);
        bestRecordText.text = $"Best Record : {(GameManager.Instance.hasRecord ? $"{GameManager.Instance.Record:0.000} Sec" : "NONE")}";
        optionPopup.SetActive(false);
        dimImg.SetActive(false);
    }

    public IEnumerator WaitForPlay()
    {
        dimImg.SetActive(true);
        yield return new WaitForSeconds(2f);
        dimImg.SetActive(false);
    }

    public void OnClickBtnRecordRestored()
    {
        if (GameManager.Instance.hasRecord)
        {
            SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

            GameManager.Instance.DeleteRecord();
            bestRecordText.text = $"Best Record : {(GameManager.Instance.hasRecord ? $"{GameManager.Instance.Record:0.000} Sec" : "NONE")}";
        }
    }

    public void OnClickBtnOption()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);
        optionPopup.SetActive(true);
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
