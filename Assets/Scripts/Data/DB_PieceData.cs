using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PieceData
{
    public enum PieceType
    {
        fish,
        seaTurtle,
        shark,
        advance,
        other,
        seaweed,
        coral
    }

    public string pieceName;
    public Sprite pieceSprite;
    public RuntimeAnimatorController animationController;
    public int oxygen;
    public int amount;
    public PieceType pieceType;
}


[CreateAssetMenu(menuName = "PieceData")]
public class DB_PieceData : ScriptableObject
{
    public List<PieceData> pieceDatas;
}
