using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FeedingData
{
    public string nameA;
    public string nameB;
    public Sprite sprite;
}


[CreateAssetMenu(menuName = "FeedingData")]
public class DB_FeedingData : ScriptableObject
{
    public List<FeedingData> feedingDatas;
}
