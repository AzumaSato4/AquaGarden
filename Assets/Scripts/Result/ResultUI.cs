using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] Transform contentParent; //VerticalLayoutGroupを持つ親
    [SerializeField] GameObject playerResultPrefab; //名前＋色用のUIプレハブ

    void Start()
    {
        foreach (var result in PlayerRanking.results)
        {
            GameObject obj = Instantiate(playerResultPrefab, contentParent);

            // 名前とスコア表示
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{result.pData.playerName} : {result.score}点";

            // プレイヤーカラー表示
            Image colorImage = obj.transform.Find("ColorPanel").GetComponent<Image>();
            colorImage.color = result.pData.color;
        }
    }
}
