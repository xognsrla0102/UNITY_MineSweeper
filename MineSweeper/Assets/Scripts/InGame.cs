using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    [SerializeField] private GameObject blockFactory;
    [SerializeField] private Transform boardGrid;
    [SerializeField] private Image backGround;

    private readonly Block[,] blockMap = new Block[15,15];

    private readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
    private readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };

    public void OnEnable()
    {
        SoundManager.Instance.PlayBGM(BGM_Type.IN_GAME);

        for (int i = 0; i < blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                blockMap[i, j] = Instantiate(blockFactory, boardGrid).GetComponent<Block>();
                blockMap[i, j].SetPos(i, j);
                blockMap[i, j].SetMap(blockMap);
            }
        }

        int bombCnt = Random.Range(25, 30);
        int nowBombCnt = 0;
        int by, bx;

        while (nowBombCnt < bombCnt)
        {
            by = Random.Range(0, blockMap.GetLength(0));
            bx = Random.Range(0, blockMap.GetLength(1));

            if (blockMap[by, bx].isBomb) continue;

            blockMap[by, bx].isBomb = true;
            nowBombCnt++;
        }

        for (int i = 0; i < blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                blockMap[i, j].SetAroundBombCnt();
            }
        }

    }

    public void OnDisable()
    {
        foreach (var block in blockMap)
            Destroy(block.GetComponent<Transform>().gameObject);
    }

    public void OnClickBlock(int y, int x)
    {
        if (blockMap[y, x].isBomb) return;

        for (int i = 0; i < 8; i++)
        {
            int ny = y + dy[i];
            int nx = x + dx[i];
            if (ny < 0 || nx < 0 || ny >= blockMap.GetLength(0) || nx >= blockMap.GetLength(1) || blockMap[ny, nx].IsBroken) continue;
            blockMap[ny, nx].OnClick();
        }
    }

    public void GameOver()
    {
        for (int i = 0; i < blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                if (blockMap[i, j].IsBroken) continue;
                blockMap[i, j].OnClick();
            }
        }
    }

    public void SetBG_Color(float remainSeconds, float limitSeconds)
    {
        float colorValue = remainSeconds / limitSeconds;
        backGround.color = new Color(colorValue, colorValue, colorValue);
    }
}
