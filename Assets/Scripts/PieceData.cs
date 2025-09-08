using UnityEngine;

[CreateAssetMenu(menuName = "PieceData")]

public class PieceData : ScriptableObject
{
    public string pieceName;
    public string category;
    public int oxygen;
    public int items;
    public bool weakfish;
}

