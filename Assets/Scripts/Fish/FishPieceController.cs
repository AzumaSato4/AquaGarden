using UnityEngine;

public class FishPieceController : MonoBehaviour
{
    public GameObject startPos;

    public void MoveToSlot(GameObject from, GameObject to)
    {
        startPos = from;
        transform.position = to.transform.position;
        
        //if (from == "AquaSlot")
        //{

        //}
    }
}
