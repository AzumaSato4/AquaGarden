using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int players = 4;
    public PieceData[] aquaPieces;
    public static bool[] avalablePieces;

    private void Start()
    {
        avalablePieces = new bool[aquaPieces.Length];

        //�e�X�g�p
        for (int i = 0; i < avalablePieces.Length; i++)
        {
            avalablePieces[i] = true;
        }
    }
}
