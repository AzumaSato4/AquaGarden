using UnityEngine;

public class RankPanelGenerator : MonoBehaviour
{
    [SerializeField] GameObject panelPrefab;
    [SerializeField] GameObject resultPanel;
    [SerializeField] ResultCameraController cameraController;

    private void Start()
    {
        for (int i = 0; i < GameManager.players; i++)
        {
            GameObject panel = Instantiate(panelPrefab, resultPanel.transform);
            RankPanel rankPanel = panel.GetComponent<RankPanel>();
            rankPanel.index = i + 1;
            rankPanel.nameText.text = GameManager.playerName[i];
            rankPanel.scoreText.text = TurnManager.scores[i].ToString();
            rankPanel.cameraController = this.cameraController;
        }
    }
}
