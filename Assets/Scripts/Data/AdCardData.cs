using UnityEngine;

[CreateAssetMenu(menuName = "AdCard")]
public class AdCardData : ScriptableObject
{
    public enum AdType
    {
        solo,
        pair,
        count
    }


    public AdType adType; //広告の種類
    public PieceData.PieceName nameA;
    public PieceData.PieceName nameB;
    public Sprite sprite;
}
