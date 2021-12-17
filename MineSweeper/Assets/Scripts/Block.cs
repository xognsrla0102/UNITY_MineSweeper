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

    [HideInInspector] public bool isBomb;

    private bool isBroken;
    public bool IsBroken
    {
        get => isBroken;
        set
        {
            isBroken = value;

            if (isBroken)
            {
                gameObject.GetComponent<Image>().color = isBomb ? new Color(1, 0.2f, 0.2f) : new Color(1, 1, 1);
                SetAroundBombCnt();
            }
        }
    }

    private bool isFlag;
    public bool IsFlag
    {
        get => isFlag;
        set
        {
            isFlag = value;
            gameObject.GetComponent<Image>().color = isFlag ? new Color(1, 245 / 255f, 0) : new Color(164 / 255f, 164 / 255f, 164 / 255f);
            aroundBombCntText.text = isFlag ? "¢Â" : "X";
        }
    }

    private int y;
    private int x;

    private readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
    private readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };

    private Block[,] blockMap;

    public void Awake()
    {
        gameObject.GetComponent<Image>().color = new Color(164 / 255f, 164 / 255f, 164 / 255f);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
            OnRightClick();
    }

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
            if (ny < 0 || nx < 0 || ny >= blockMap.GetLength(0) || nx >= blockMap.GetLength(1)) continue;
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
        IsBroken = true;
        if (AroundBombCnt != 0) return;

        InGame inGame = transform.parent.parent.parent.GetComponent<InGame>();
        inGame.OnClickBlock(y, x);
    }

    public void OnRightClick()
    {
        if (!isBroken) IsFlag = !IsFlag;
    }
}
