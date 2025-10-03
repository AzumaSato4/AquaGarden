using UnityEngine;

public class AquariumBoard : MonoBehaviour
{
    //水族館ボードにあるオブジェクトを格納
    public GameObject[] aquaTiles;
    public GameObject[] aquaSlots;
    public GameObject storage;
    public GameObject[] CoinSpots;
    public GameObject coin;

    public bool[] isPlayer;

    private void Awake()
    {
        isPlayer = new bool[6];

        for (int i = 0; i < 6; i++)
        {
            isPlayer[i] = false;
        }
    }
}
