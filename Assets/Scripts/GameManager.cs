using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int players = 4;
    [SerializeField] PieceData[] aquaPiecesEditor;
    public static PieceData[] aquaPieces;
    public static bool[] avalablePieces;

    private void Awake()
    {
        aquaPieces = new PieceData[aquaPiecesEditor.Length];
        Array.Copy(aquaPiecesEditor, aquaPieces, aquaPiecesEditor.Length);
        avalablePieces = new bool[aquaPieces.Length];

        //テスト用
        for (int i = 0; i < avalablePieces.Length; i++)
        {
            avalablePieces[i] = true;
        }
    }
}
