using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainMenu;
    public InGame inGame;

    public void Awake()
    {
        Instance = this;

        if (!mainMenu.activeSelf) mainMenu.SetActive(true);
        if (inGame.gameObject.activeSelf) inGame.gameObject.SetActive(false);
    }

    public void OnClickBtnPlay()
    {
        mainMenu.SetActive(false);
        inGame.gameObject.SetActive(true);
    }

    public void OnClickBtnBack()
    {
        mainMenu.SetActive(true);
        inGame.gameObject.SetActive(false);
    }
}
