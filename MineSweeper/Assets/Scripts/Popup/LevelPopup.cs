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

        // lvType�� LevelType������ ��ȯ ����� ��[����Ƽ���� Ư�� �ڷ����� ��ư �Ű����� �ν��ؼ�]
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
