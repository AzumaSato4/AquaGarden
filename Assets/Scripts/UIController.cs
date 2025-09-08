using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject START;
    public GameObject NumberOfPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( GameController.gameMode == "TopMenu")
        {
            START.SetActive(true);
            NumberOfPlayer.SetActive(true);
        }
        else if (GameController.gameMode == "Start" || GameController.gameMode == "Playing")
        {
            START.SetActive(false);
            NumberOfPlayer.SetActive(false);
        }
    }
}
