using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public GameObject panel0;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    //表示しているパネルを保持
    public string mainPanel;

    void Start()
    {
        //最初はpanel0だけ表示する
        panel0.SetActive(true);
        //他は非表示
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        mainPanel = "Panel0";
    }
}
