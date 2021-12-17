using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameManager();
            return instance;
        }
    }

    public GameObject mainMenu;
    public GameObject inGame;

    public void Awake()
    {
        if (!mainMenu.activeSelf) mainMenu.SetActive(true);
        if (inGame.activeSelf) inGame.SetActive(false);
    }

    public void OnClickBtnPlay()
    {
        mainMenu.SetActive(false);
        inGame.SetActive(true);
    }

    public void OnClickBtnBack()
    {
        mainMenu.SetActive(true);
        inGame.SetActive(false);
    }
}
