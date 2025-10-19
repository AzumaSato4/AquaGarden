using UnityEngine;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] GameObject maskPanel;

    private void Start()
    {
        maskPanel.SetActive(false);
    }

    public void ActiveMask()
    {
        maskPanel.SetActive(true);
    }
}
