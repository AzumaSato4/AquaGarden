using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] Transform contentParent; //VerticalLayoutGroup�����e
    [SerializeField] GameObject playerResultPrefab; //���O�{�F�p��UI�v���n�u

    void Start()
    {
        foreach (var result in PlayerRanking.results)
        {
            GameObject obj = Instantiate(playerResultPrefab, contentParent);

            // ���O�ƃX�R�A�\��
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{result.pData.playerName} : {result.score}�_";

            // �v���C���[�J���[�\��
            Image colorImage = obj.transform.Find("ColorPanel").GetComponent<Image>();
            colorImage.color = result.pData.color;
        }
    }
}
