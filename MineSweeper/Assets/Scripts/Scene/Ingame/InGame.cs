using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

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

    [SerializeField] private PostProcessProfile profile;
    private ChromaticAberration chromaticAberration;
    private Grain grain;
    private LensDistortion lensDistortion;

    private List<Vector2> bombsPos = new List<Vector2>();

    [HideInInspector] public LevelType levelType;

    public void Awake()
    {
        chromaticAberration = profile.GetSetting<ChromaticAberration>();
        grain = profile.GetSetting<Grain>();
        lensDistortion = profile.GetSetting<LensDistortion>();
    }

    public void OnEnable()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.IN_GAME);

        chromaticAberration.intensity.value = 0f;
        grain.intensity.value = 0f;
        lensDistortion.intensity.value = 0f;

        // 블럭 생성
        for (int i = 0; i < Utility.SIZEY; i++)
        {
            for (int j = 0; j < Utility.SIZEX; j++)
            {
                blockMap[i, j] = Instantiate(blockFactory, boardGrid).GetComponent<Block>();
                blockMap[i, j].SettingBlock(i, j, blockMap);
            }
        }

        int bombCnt = 0;

        switch (levelType)
        {
            case LevelType.GOSU: bombCnt = Utility.bombGosuCnt; break;
            case LevelType.HUMAN: bombCnt = Utility.bombHumanCnt; break;
            case LevelType.NOOB: bombCnt = Utility.bombNoobCnt; break;
            default: Debug.Assert(false); break;
        }

        int nowBombCnt = 0;
        int by, bx;

        // 지뢰 생성
        while (nowBombCnt < bombCnt)
        {
            by = Random.Range(0, Utility.SIZEY);
            bx = Random.Range(0, Utility.SIZEX);

            if (blockMap[by, bx].isBomb) continue;

            bombsPos.Add(new Vector2(bx, by));
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

        bombsPos.Clear();
    }

    public void OnClickBlock(int y, int x)
    {
        for (int i = 0; i < Utility.BLOCK_DIR; i++)
        {
            int ny = y + Utility.dy[i];
            int nx = x + Utility.dx[i];
            if (ny < 0 || nx < 0 || ny >= Utility.SIZEY || nx >= Utility.SIZEX || blockMap[ny, nx].isBomb) continue;
            blockMap[ny, nx].OnClick();
        }
    }

    public void OnClick3x3(int y, int x)
    {
        for (int i = 0; i < Utility.BLOCK_DIR; i++)
        {
            int ny = y + Utility.dy[i];
            int nx = x + Utility.dx[i];
            if (ny < 0 || nx < 0 || ny >= Utility.SIZEY || nx >= Utility.SIZEX || blockMap[ny, nx].BlockType == BlockType.FLAG) continue;
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

    public void GameOver(bool isTimeOver)
    {
        isResult = true;
        waitForResult = true;

        ingameTimer.StopTimer();

        StartCoroutine(GameOverCoroutine(isTimeOver));
    }

    private IEnumerator GameOverCoroutine(bool isTimeOver)
    {
        if (isTimeOver)
        {
            yield return new WaitForSeconds(5f);
            SoundManager.Instance.StopBGM();
        }
        else
        {
            SoundManager.Instance.StopBGM();
            yield return new WaitForSeconds(3f);
        }

        foreach (var bombPos in bombsPos)
        {
            blockMap[(int)bombPos.y, (int)bombPos.x].BreakAfterGameOver();
            yield return new WaitForSeconds(0.02f);
        }

        SoundManager.Instance.PlayBGM(BGM_Type.GAME_FAIL);
        yield return new WaitForSeconds(3f);

        resultPopup.SetActive(true);
        waitForResult = false;

        timeText.text = $"Record [Now : FAILED] / [Best : [{(GameManager.Instance.hasRecord ? $"{GameManager.Instance.Record:0.000}" : "NO-RECORD")}]";
    }

    public void SetBG_Color(Color color)
    {
        backGround.color = color;
    }

    public void ScreenBlackEffect()
    {
        StartCoroutine(ScreenBlackEffectCoroutine());
    }

    private IEnumerator ScreenBlackEffectCoroutine()
    {
        chromaticAberration.intensity.value = 0.5f;
        lensDistortion.intensity.value = -100f;

        do
        {
            chromaticAberration.intensity.Interp(chromaticAberration.intensity.value, 0f, Time.deltaTime);
            lensDistortion.intensity.Interp(lensDistortion.intensity.value, 0f, Time.deltaTime);
            yield return null;
        } while (chromaticAberration.intensity.value >= 0.001f);

        chromaticAberration.intensity.value = 0f;
        lensDistortion.intensity.value = 0f;
    }

    public void ScreenGrainEffect()
    {
        StartCoroutine(ScreenGrainEffectCoroutine());
    }

    private IEnumerator ScreenGrainEffectCoroutine()
    {
        do
        {
            grain.intensity.Interp(grain.intensity.value, 1f, Time.deltaTime / 5f);
            yield return null;
        } while (grain.intensity.value <= 0.9f);

        grain.intensity.value = 1f;
    }

    public void OnClickBtnBack()
    {
        // 게임 오버 모션 중에는 나갈 수 없음
        if (waitForResult) return;

        SoundManager.Instance.PlaySFX(SFX_Type.ON_CLICK);

        resultPopup.SetActive(false);

        GameManager.Instance.mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
