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
                    aroundBombCntText.text = $"{aroundBombCnt}";
                    break;
                case BlockType.FLAG: // 깃발
                    aroundBombCntText.text = "★";
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

    public void OnClick()
    {
        // 안 깨졌거나, 의문 블럭이 아닌 경우엔 클릭해도 반응 없음
        if (BlockType != BlockType.UNBROKEN && BlockType != BlockType.QUESTION) return;

        BlockType = BlockType.BROKEN;

        // 폭탄일 경우 게임 오버
        //if ()
        //{
        //    return;
        //}

        // 주변에 지뢰가 있을 경우 탈출
        if (aroundBombCnt != 0) return;
        // 주변 블럭 깸
        GameManager.Instance.inGame.OnClickBlock(y, x);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
            OnRightClick();
    }

    public void OnRightClick()
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
}
