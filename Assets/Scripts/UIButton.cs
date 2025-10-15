using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] UIController.PanelType panelType;

    public void OnUIButton()
    {
        uiController.ChangeUI(panelType);
    }
}
