using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public MainMenu mainMenu;
    public InGame inGame;

    public bool hasRecord => PlayerPrefs.HasKey("Record");
    public float Record
    {
        get { return hasRecord ? PlayerPrefs.GetFloat("Record") : 999f; }
        set { PlayerPrefs.SetFloat("Record", value); }
    }

    public void Start()
    {
        if (!mainMenu.gameObject.activeSelf) mainMenu.gameObject.SetActive(true);
        if (inGame.gameObject.activeSelf) inGame.gameObject.SetActive(false);
    }

    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("Record");
    }

    public void OnClickBtnPlay()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

        StartCoroutine(WaitForPlay());
    }

    private IEnumerator WaitForPlay()
    {
        yield return mainMenu.WaitForPlay();
        mainMenu.gameObject.SetActive(false);
        inGame.gameObject.SetActive(true);
    }

    public void OnClickBtnBack()
    {
        // 게임 오버 모션 중에는 나갈 수 없음
        if (inGame.waitForResult) return;

        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

        inGame.resultPopup.SetActive(false);

        mainMenu.gameObject.SetActive(true);
        inGame.gameObject.SetActive(false);
    }
}
