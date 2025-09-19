using UnityEngine;

[CreateAssetMenu(menuName = "FishData")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite icon;
    public Color color;
    public bool isSeaweed;
}