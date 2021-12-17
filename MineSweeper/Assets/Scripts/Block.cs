using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Text aroundBombCntText;

    private int aroundBombCnt;
    public int AroundBombCnt
    {
        get => aroundBombCnt;
        set
        {
            aroundBombCnt = value;

            SetAroundBombCntText();
        }
    }

    public bool isBomb;

    private bool isBroken;
    public bool IsBroken
    {
        get => isBroken;
        set
        {
            SetAroundBombCntText();
        }
    }

    private int y;
    private int x;

    private readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
    private readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };

    private Block[,] blockMap;

    public void SetPos(int y, int x)
    {
        this.y = y;
        this.x = x;
    }

    public void SetMap(Block[,] map)
    {
        blockMap = map;
    }

    public void SetAroundBombCnt()
    {
        int bombCnt = 0;

        for (int i = 0; i < 8; i++)
        {
            int ny = y + dy[i];
            int nx = x + dx[i];
            if (ny < 0 || nx < 0 || ny >= 9 || nx >= 9) continue;
            if (blockMap[ny, nx].isBomb) bombCnt++;
        }

        AroundBombCnt = bombCnt;
    }

    public void SetAroundBombCntText()
    {
        if (IsBroken)
        {
            if (isBomb) aroundBombCntText.text = "!";
            else aroundBombCntText.text = $"{aroundBombCnt}";
        }
        else aroundBombCntText.text = "X";
    }

    public void OnClick()
    {
        isBroken = true;

        InGame inGame = transform.parent.parent.parent.GetComponent<InGame>();
        inGame.OnClickBlock(y, x);
    }
}
