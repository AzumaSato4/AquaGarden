using UnityEngine;

public class SelectPlayers : MonoBehaviour
{
    public PlayerManager playerManager;

    public void SetPlayers(int n)
    {
        PlayerManager.players = n;
    }

}
