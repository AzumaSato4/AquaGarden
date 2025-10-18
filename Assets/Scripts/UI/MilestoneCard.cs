using UnityEngine;
using UnityEngine.UI;

public class MilestoneCard : MonoBehaviour
{
    public Image cardImage;
    public GameObject checkerPrefab;
    [SerializeField] GameObject container;
    [SerializeField] GameObject mask;
    bool isOpened;

    private void Start()
    {
        isOpened = !GameManager.isSecretMode;
        if (isOpened)
        {
            Destroy(mask);
        }
    }

    public void SetChecker(int index)
    {
        GameObject obj =  Instantiate(checkerPrefab, container.transform);
        obj.GetComponent<Image>().sprite = GameManager.milestoneColor[index];

        if (!isOpened)
        {
            Destroy(mask);
        }
    }
}
