using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    private BlockType blockType;
    public BlockType BlockType
    {
        get => blockType;
        set
        {
            blockType = value;
            gameObject.GetComponent<Image>().color = Utility.BLOCK_COLORS[(int)blockType];

            switch (blockType)
            {
                case BlockType.UNBROKEN: // 기본 상태
                    aroundBombCntText.text = "━";
                    break;
                case BlockType.BROKEN: // 깨지면 주변 지뢰 수 표기
                    aroundBombCntText.text = aroundBombCnt == 0 ? string.Empty : $"{aroundBombCnt}";
                    break;
                case BlockType.FLAG: // 깃발
                    aroundBombCntText.text = "※";
                    break;
                case BlockType.QUESTION: // 의문
                    aroundBombCntText.text = "?";
                    break;
                case BlockType.BOMB: // 폭탄
                    aroundBombCntText.text = "!";
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    private int x, y;
    private Block[,] blockMap;

    [SerializeField] private Text aroundBombCntText;

    private int aroundBombCnt;

    public bool isBomb;

    [SerializeField] private GameObject breakEffect;

    public void SettingBlock(int y, int x, Block[,] blockMap)
    {
        this.x = x;
        this.y = y;
        this.blockMap = blockMap;
        BlockType = BlockType.UNBROKEN;
    }

    // 8 방향에서 지뢰 몇개 있는지 확인
    public void SetAroundBombCnt()
    {
        int bombCnt = 0;
        for (int i = 0; i < Utility.BLOCK_DIR; i++)
        {
            int ny = y + Utility.dy[i];
            int nx = x + Utility.dx[i];
            if (ny < 0 || nx < 0 || ny >= Utility.SIZEY || nx >= Utility.SIZEX) continue;
            if (blockMap[ny, nx].isBomb) bombCnt++;
        }
        aroundBombCnt = bombCnt;
    }

    private int GetAroundFlagCnt()
    {
        int flagCnt = 0;
        for (int i = 0; i < Utility.BLOCK_DIR; i++)
        {
            int ny = y + Utility.dy[i];
            int nx = x + Utility.dx[i];
            if (ny < 0 || nx < 0 || ny >= Utility.SIZEY || nx >= Utility.SIZEX) continue;
            if (blockMap[ny, nx].BlockType == BlockType.FLAG) flagCnt++;
        }
        return flagCnt;
    }

    public void BreakAfterGameOver()
    {
        GameObject breakEffect = Instantiate(this.breakEffect);
        breakEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);

        BlockType = BlockType.BOMB;
    }

    public void OnMouseOver()
    {
        if (GameManager.Instance.inGame.isResult) return;

        if (MouseInput.Instance.LeftClick())
            OnClick();
        if (MouseInput.Instance.RightClick())
            OnRightClick();
        if (MouseInput.Instance.MiddleClick())
            OnMiddleClick();
    }

    public void OnClick()
    {
        // 결과 창일 경우 클릭 못함
        if (GameManager.Instance.inGame.isResult) return;

        // 안 깼거나, 의문 블럭만 부술 수 있음
        if (BlockType != BlockType.UNBROKEN && BlockType != BlockType.QUESTION) return;

        GameObject breakEffect = Instantiate(this.breakEffect);
        breakEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);

        // 폭탄일 경우 게임 오버
        if (isBomb)
        {
            SoundManager.Instance.PlaySFX(SFX_Type.BOMB);
            BlockType = BlockType.BOMB;
            GameManager.Instance.inGame.GameOver();
            return;
        }

        SFX_Type breakBlockSFX = (SFX_Type)Random.Range((int)SFX_Type.BREAK_BLOCK_1, (int)SFX_Type.BREAK_BLOCK_4 + 1);
        SoundManager.Instance.PlaySFX(breakBlockSFX);
        BlockType = BlockType.BROKEN;
        GameManager.Instance.inGame.RemainBlockCnt--;

        // 주변에 지뢰가 있을 경우 탈출
        if (aroundBombCnt != 0) return;

        // 주변 8블럭에 지뢰 없으면 블럭 깸
        GameManager.Instance.inGame.OnClickBlock(y, x);
    }

    private void OnRightClick()
    {
        switch (BlockType)
        {
            case BlockType.UNBROKEN:
                BlockType = BlockType.FLAG;
                break;
            case BlockType.FLAG:
                BlockType = BlockType.QUESTION;
                break;
            case BlockType.QUESTION:
                BlockType = BlockType.UNBROKEN;
                break;
        }
    }

    private void OnMiddleClick()
    {
        if (BlockType != BlockType.BROKEN) return;
        if (aroundBombCnt != GetAroundFlagCnt()) return;
        GameManager.Instance.inGame.OnClick3x3(y, x);
    }
}
