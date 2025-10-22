using UnityEngine;
using UnityEngine.UI;

public class MovedPanel : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;

    private void Start()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            gridLayoutGroup.constraintCount = 2;
            gridLayoutGroup.cellSize = new Vector2(160f, 180f);
        }
    }
}
