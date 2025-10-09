using UnityEngine;

[CreateAssetMenu(menuName = "Feeding")]
public class FeedingData : ScriptableObject
{
    public PieceData.PieceName nameA;
    public PieceData.PieceName nameB;
    public Sprite sprite;
}
