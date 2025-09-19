using System.Collections.Generic;
using UnityEngine;

public class SeaFish : MonoBehaviour
{
    public GameManager manager;

    public SeaBoard sea;
    public List<FishData> seaFishes;
    public List<int> seaFishCounts;


    //ƒQ[ƒ€ŠJn‚É‰Šú‰»
    public void Initialze(List<FishData> fishTypes, List<int> counts)
    {
        //‹›‹î‚Ìí—Ş
        //‚»‚Ìí—Ş‚ÌŒÂ”
        for (int i = 0; i < fishTypes.Count; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaFishCounts[i] = counts[i];
        }
    }
}
