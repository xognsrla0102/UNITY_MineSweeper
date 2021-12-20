using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    [SerializeField] private GameObject blockFactory;
    [SerializeField] private Transform boardGrid;
    [SerializeField] private Image backGround;
    
    private readonly Block[,] blockMap = new Block[Utility.SIZEY, Utility.SIZEX];

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

        int bombCnt = Random.Range(25, 30);
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

    public void GameOver()
    {
        for (int i = 0; i < Utility.SIZEY; i++)
        {
            for (int j = 0; j < Utility.SIZEX; j++)
            {
                if (blockMap[i, j].BlockType == BlockType.BROKEN) continue;
                blockMap[i, j].OnClick();
            }
        }
    }

    public void SetBG_Color(Color color)
    {
        backGround.color = color;
    }
}
