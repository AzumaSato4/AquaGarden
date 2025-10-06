using UnityEngine;

[CreateAssetMenu(menuName = "AdCardData")]
public class AdCardData : ScriptableObject
{
    public int adID; //広告ID
    public string nameA;
    public string nameB;
    public Sprite sprite;
}
