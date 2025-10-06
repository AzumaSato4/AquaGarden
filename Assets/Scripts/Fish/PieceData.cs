using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "PieceData")]
public class PieceData : ScriptableObject
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
    public AnimatorController animation;
    public int oxygen;
    public int amount;
    public PieceType pieceType;
}
