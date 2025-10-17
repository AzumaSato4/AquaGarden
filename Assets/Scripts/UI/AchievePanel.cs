using UnityEngine;

public class AchievePanel : MonoBehaviour
{
    [SerializeField] GameObject rewardPanel;
    public static bool isReward;

    private void OnEnable()
    {
        if (isReward)
        {
            rewardPanel.SetActive(true);
            isReward = false;
        }
        else
        {
            rewardPanel.SetActive(false);
        }
    }
}
