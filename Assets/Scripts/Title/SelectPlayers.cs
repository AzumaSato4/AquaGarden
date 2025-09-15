using JetBrains.Annotations;
using TMPro;
using UnityEngine;


public class SelectPlayers : MonoBehaviour
{
    public PlayerManager playerManager;

    private void Start()
    {
        
    }

    public void SetPlayers(int n)
    {
        PlayerManager.players = n;


        //2人の時は「Gossを3pにする」
        if (n == 2) n++;

        //人数に応じて配列の長さを変える
        System.Array.Resize(ref PlayerManager.pName, n);
        System.Array.Resize(ref PlayerManager.pColor, n);
    }

}
