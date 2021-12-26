using UnityEngine;

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

    public bool hasRecord => PlayerPrefs.HasKey("Record");
    public float Record
    {
        get { return hasRecord ? PlayerPrefs.GetFloat("Record") : 999f; }
        set { PlayerPrefs.SetFloat("Record", value); }
    }

    public MainMenu mainMenu;
    public InGame inGame;

    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("Record");
    }
}
