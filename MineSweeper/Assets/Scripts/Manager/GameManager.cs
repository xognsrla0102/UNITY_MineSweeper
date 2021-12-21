using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainMenu;
    public InGame inGame;

    public bool hasRecord => PlayerPrefs.HasKey("Record");
    public float Record
    {
        get { return hasRecord ? PlayerPrefs.GetFloat("Record") : 999f; }
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

    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("Record");
    }

    public void OnClickBtnPlay()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);
        mainMenu.SetActive(false);
        inGame.gameObject.SetActive(true);
    }

    public void OnClickBtnBack()
    {
        // 게임 오버 모션 중에는 나갈 수 없음
        if (inGame.waitForResult) return;

        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

        inGame.resultPopup.SetActive(false);

        mainMenu.SetActive(true);
        inGame.gameObject.SetActive(false);
    }
}
