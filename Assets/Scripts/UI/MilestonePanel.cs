using UnityEngine;

public class MilestonePanel : MonoBehaviour
{
    [SerializeField] GameObject milestoneCardPrefab;
    GameManager gameManager;

    public static GameObject[] milestoneCards;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void Initialize()
    {
        milestoneCards = new GameObject[TurnManager.milestones.Length];
        for (int i = 0; i < TurnManager.milestones.Length; i++)
        {
            GameObject obj = Instantiate(milestoneCardPrefab, transform);
            MilestoneCard card = obj.GetComponent<MilestoneCard>();
            Sprite cardSprite = TurnManager.milestones[i].sprite;
            card.cardImage.sprite = cardSprite;
            milestoneCards[i] = obj;
        }
    }

    public static void SetChecker(int playerNum, int index)
    {
        milestoneCards[index].GetComponent<MilestoneCard>().SetChecker(playerNum - 1);
    }
}
