using UnityEngine;
public class MouseInput : MonoBehaviour
{
    public static MouseInput Instance;

    public void Awake()
    {
        Instance = this;
    }

    public bool LeftClick() => Input.GetMouseButtonUp(0);
    public bool RightClick() => Input.GetMouseButtonUp(1);
    public bool MiddleClick() => Input.GetMouseButtonDown(2);
}
