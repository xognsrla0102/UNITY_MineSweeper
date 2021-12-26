using UnityEngine;
using System.Collections;

public enum LevelType
{
    GOSU,
    HUMAN,
    NOOB,
    LEVEL_NUM
}

public class LevelPopup : MonoBehaviour
{
    [SerializeField] private GameObject dimPlayImg;

    public void OnEnable()
    {
        dimPlayImg.SetActive(false);
    }    

    public void OnClickBtnBack()
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);
        gameObject.SetActive(false);
    }

    public void OnClickPlayBtn(int lvType)
    {
        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

        // lvType을 LevelType형으로 변환 해줘야 함[유니티에서 특정 자료형만 버튼 매개변수 인식해서]
        GameManager.Instance.inGame.levelType = (LevelType)lvType;
        StartCoroutine(WaitForPlay());
    }

    private IEnumerator WaitForPlay()
    {
        dimPlayImg.SetActive(true);
        yield return new WaitForSeconds(2f);
        dimPlayImg.SetActive(false);

        gameObject.SetActive(false);
        GameManager.Instance.mainMenu.gameObject.SetActive(false);
        GameManager.Instance.inGame.gameObject.SetActive(true);
    }
}
