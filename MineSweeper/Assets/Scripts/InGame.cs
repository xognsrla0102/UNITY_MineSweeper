using UnityEngine;

public class InGame : MonoBehaviour
{
    [SerializeField] private GameObject blockFactory;
    [SerializeField] private Transform boardGrid;

    private readonly Block[,] blockMap = new Block[9,9];

    private readonly int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
    private readonly int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };

    public void OnEnable()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                blockMap[i, j] = Instantiate(blockFactory, boardGrid).GetComponent<Block>();
                blockMap[i, j].SetPos(i, j);
                blockMap[i, j].SetMap(blockMap);
            }
        }

        int bombCnt = Random.Range(5, 8);
        int nowBombCnt = 0;
        int by, bx;

        while (nowBombCnt < bombCnt)
        {
            by = Random.Range(0, 9);
            bx = Random.Range(0, 9);

            if (blockMap[by, bx].isBomb) continue;

            blockMap[by, bx].isBomb = true;
            nowBombCnt++;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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
            if (ny < 0 || nx < 0 || ny >= 9 || nx >= 9) continue;

            if (blockMap[ny, nx].AroundBombCnt == 0)
            {
                
            }
        }
    }
}
