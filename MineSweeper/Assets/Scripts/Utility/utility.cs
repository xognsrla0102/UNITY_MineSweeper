using UnityEngine;
public static class Utility
{
    public const int BLOCK_DIR = 8;
    public static int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
    public static int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };

    public const int SIZEY = 30;
    public const int SIZEX = 30;

    public const int bombMinCnt = 100;
    public const int bombMaxCnt = 110;

    public static Color[] BLOCK_COLORS = new Color[(int)BlockType.COUNT]
    {
        new Color32(0xA4, 0xA4, 0xA4, 0xFF), // UNBROKEN
        Color.white, // BROKEN
        new Color32(0xFF, 0xF5, 0x00, 0xFF), // FLAG
        new Color32(0x7D, 0xFF, 0x00, 0xFF), // QUESTION
        new Color32(0xFF, 0x33, 0x33, 0xFF) // BOMB
    };
}
