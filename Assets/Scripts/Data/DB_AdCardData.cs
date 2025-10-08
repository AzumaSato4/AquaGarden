using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdCardData
{
    public enum AdType
    {
        solo,
        pair,
        count
    }


    public AdType adType; //広告の種類
    public string nameA;
    public string nameB;
    public Sprite sprite;
}


[CreateAssetMenu(menuName = "AdCardData")]
public class DB_AdCardData : ScriptableObject
{
    public List<AdCardData> adCardDatas;
}
