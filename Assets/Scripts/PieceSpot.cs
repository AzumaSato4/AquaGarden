using UnityEngine;

public class PieceSpot : MonoBehaviour
{
    public bool available = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("FishPiece"))
        {
            available = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.CompareTag("FishPiece"))
        {
            available = true;
        }
    }
}
