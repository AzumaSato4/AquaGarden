using UnityEngine;

[CreateAssetMenu(menuName = "Piece")]
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

    public enum PieceName
    {
        SmallFish,
        LargeFish,
        SeaTurtle,
        Seahorse,
        Shark,
        WhaleShark,
        Seaweed,
        Coral,
        Flapjack,
        Manta,
        Remora
    }

    public PieceName pieceName;
    public Sprite pieceSprite;
    public RuntimeAnimatorController animationController;
    public int oxygen;
    public int amount;
    public PieceType pieceType;
    public bool isMilestone;
}
