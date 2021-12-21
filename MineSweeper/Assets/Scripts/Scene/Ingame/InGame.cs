using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGame : MonoBehaviour
{
    public GameObject resultPopup;
    [SerializeField] private Text timeText;
    [SerializeField] private Timer ingameTimer;

    [SerializeField] private Text remainBlock;
    private int remainBlockCnt;
    [HideInInspector] public int RemainBlockCnt
    {
        get { return remainBlockCnt; }
        set
        {
            remainBlockCnt = value;
            remainBlock.text = $"Remain Block\n=> {remainBlockCnt}";

            if (remainBlockCnt == 0)
                GameClear();
        }
    }

    [SerializeField] private GameObject blockFactory;
    [SerializeField] private Transform boardGrid;
    [SerializeField] private Image backGround;
    
    private readonly Block[,] blockMap = new Block[Utility.SIZEY, Utility.SIZEX];

    [HideInInspector] public bool isResult;
    [HideInInspector] public bool waitForResult;

    public void OnEnable()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.IN_GAME);

        // 블럭 생성
        for (int i = 0; i < Utility.SIZEY; i++)
        {
            for (int j = 0; j < Utility.SIZEX; j++)
            {
                blockMap[i, j] = Instantiate(blockFactory, boardGrid).GetComponent<Block>();
                blockMap[i, j].SettingBlock(i, j, blockMap);
            }
        }

        int bombCnt = Random.Range(50, 60);
        int nowBombCnt = 0;
        int by, bx;

        // 지뢰 생성
        while (nowBombCnt < bombCnt)
        {
            by = Random.Range(0, Utility.SIZEY);
            bx = Random.Range(0, Utility.SIZEX);

            if (blockMap[by, bx].isBomb) continue;
            blockMap[by, bx].isBomb = true;
            nowBombCnt++;
        }

        // 블럭 숫자 초기화
        for (int i = 0; i < Utility.SIZEY; i++)
            for (int j = 0; j < Utility.SIZEX; j++)
                blockMap[i, j].SetAroundBombCnt();

        resultPopup.SetActive(false);

        isResult = false;
        waitForResult = false;

        RemainBlockCnt = Utility.SIZEX * Utility.SIZEY - bombCnt;
    }

    public void OnDisable()
    {
        foreach (var block in blockMap)
            Destroy(block.GetComponent<Transform>().gameObject);
    }

    public void OnClickBlock(int y, int x)
    {
        for (int i = 0; i < Utility.BLOCK_DIR; i++)
        {
            int ny = y + Utility.dy[i];
            int nx = x + Utility.dx[i];
            if (ny < 0 || nx < 0 || ny >= Utility.SIZEY || nx >= Utility.SIZEX || blockMap[ny, nx].BlockType == BlockType.BOMB) continue;
            blockMap[ny, nx].OnClick();
        }
    }
    
    public void GameClear()
    {
        isResult = true;
        ingameTimer.StopTimer();
        StartCoroutine(GameClearCoroutine());
    }

    private IEnumerator GameClearCoroutine()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.GAME_CLEAR);

        waitForResult = true;
        ingameTimer.IsBlack = false;

        yield return new WaitForSeconds(5f);
        waitForResult = false;

        resultPopup.SetActive(true);

        float nowRecord = ingameTimer.TotalTime / 1000f;
        float bestRecord = GameManager.Instance.Record;

        if (nowRecord < bestRecord)        
            bestRecord = GameManager.Instance.Record = nowRecord;

        timeText.text = $"Record [Now : {nowRecord:0.000}] / [Best : {bestRecord:0.000}]";
    }

    public void GameOver()
    {
        SoundManager.Instance.StopBGM();

        isResult = true;
        waitForResult = true;

        ingameTimer.StopTimer();

        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < Utility.SIZEY; i++)
        {
            for (int j = 0; j < Utility.SIZEX; j++)
            {
                blockMap[i, j].BreakAfterGameOver();
                yield return new WaitForSeconds(0.02f);
            }
        }

        SoundManager.Instance.PlayBGM(BGM_Type.GAME_FAIL);
        yield return new WaitForSeconds(3f);

        waitForResult = false;

        resultPopup.SetActive(true);
        timeText.text = $"Record [Now : FAILED] / [Best : [{(GameManager.Instance.hasRecord ? $"{GameManager.Instance.Record:0.000}" : "NO-RECORD")}]";
    }

    public void SetBG_Color(Color color)
    {
        backGround.color = color;
    }
}
