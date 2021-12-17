using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject mainMenu;

    private void Awake()
    {
        instance = new GameManager();
    }

    public void OnClickBtnPlay()
    {
        
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
