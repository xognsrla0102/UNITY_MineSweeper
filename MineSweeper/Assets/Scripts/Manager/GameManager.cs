using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainMenu;
    public InGame inGame;

    public float Record
    {
        get { return PlayerPrefs.GetFloat("Record"); }
        set { PlayerPrefs.SetFloat("Record", value); }
    }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        if (!mainMenu.activeSelf) mainMenu.SetActive(true);
        if (inGame.gameObject.activeSelf) inGame.gameObject.SetActive(false);
    }

    public void OnClickBtnPlay()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);
        mainMenu.SetActive(false);
        inGame.gameObject.SetActive(true);
    }

    public void OnClickBtnBack()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);
        mainMenu.SetActive(true);
        inGame.gameObject.SetActive(false);
    }
}
