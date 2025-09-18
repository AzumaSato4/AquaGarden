using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public string playerName;   //プレイヤー名
    public Color color;         //プレイヤーカラー
    public int money;           //所持資金
}
