using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public GameObject panel0;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    //�\�����Ă���p�l����ێ�
    public string mainPanel;

    void Start()
    {
        //�ŏ���panel0�����\������
        panel0.SetActive(true);
        //���͔�\��
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        mainPanel = "Panel0";
    }
}
