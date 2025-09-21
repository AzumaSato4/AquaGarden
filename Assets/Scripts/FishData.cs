using UnityEngine;

[CreateAssetMenu(menuName = "FishData")]
public class FishData : ScriptableObject
{
    public enum Type
    {
        Seaweed,
        Coral,
        SmallFish,
        LargeFish,
        Seaturtle,
        Seahorse,
        Shark,
        WhaleShark
    }
    public int id;
    public Type type;
    public Sprite icon;
    public Color color;
    public int oxygen;
}