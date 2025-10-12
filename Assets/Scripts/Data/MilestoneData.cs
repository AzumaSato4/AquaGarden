using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Milestone")]
public class MilestoneData : ScriptableObject
{
    public enum MilestoneType
    {
        fish,
        seaturtle,
        seahorse,
        shark
    }

    public MilestoneType type;
    public List<PieceData> rewards;
    public List<PieceData.PieceName> conditions;
}
