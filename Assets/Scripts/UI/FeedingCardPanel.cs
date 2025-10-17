using UnityEngine;

public class FeedingCardPanel : MonoBehaviour
{
    [SerializeField] GameObject feedingCardPrefab;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void Initialize(FeedingData data, string name)
    {
        GameObject obj =Instantiate(feedingCardPrefab, transform);
        FeedingCard card = obj.GetComponent<FeedingCard>();
        card.playerName.text = name;
        card.cardImage.sprite = data.sprite;
    }
}
