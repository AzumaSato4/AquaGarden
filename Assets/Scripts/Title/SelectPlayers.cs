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


        //2�l�̎��́uGoss��3p�ɂ���v
        if (n == 2) n++;

        //�l���ɉ����Ĕz��̒�����ς���
        System.Array.Resize(ref PlayerManager.pName, n);
        System.Array.Resize(ref PlayerManager.pColor, n);
    }

}
