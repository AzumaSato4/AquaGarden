using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "PieceData")]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public Sprite pieceSprite;
    public AnimatorController animation;
    public int oxgen;
}
