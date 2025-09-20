using UnityEngine;

[CreateAssetMenu(menuName = "FishData")]
public class FishData : ScriptableObject
{
    public int id;
    public string fishName;
    public Sprite icon;
    public Color color;
    public int oxygen;
}