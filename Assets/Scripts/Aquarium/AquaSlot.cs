using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    public GameObject[] pieceSpots = new GameObject[7];
    public bool selectable = false;

    public GameObject CheckSpot()
    {
        foreach (GameObject spot in pieceSpots)
        {
            if (spot.GetComponent<PieceSpot>().available)
            {
                return spot;
            }
        }

        return null;
    }
}
