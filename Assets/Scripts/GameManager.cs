using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int players = 4;
    [SerializeField] PieceData[] aquaPiecesEditor;
    [SerializeField] PieceData[] adAquaPiecesEditor;
    public static PieceData[] aquaPieces;
    public static PieceData[] adAquaPieces;
    public static bool[] avalableAdPieces;

    private void Start()
    {
        Application.targetFrameRate = 60;

        aquaPieces = new PieceData[aquaPiecesEditor.Length];
        Array.Copy(aquaPiecesEditor, aquaPieces, aquaPiecesEditor.Length);

        adAquaPieces = new PieceData[adAquaPiecesEditor.Length];
        Array.Copy(adAquaPiecesEditor, adAquaPieces, adAquaPiecesEditor.Length);
        avalableAdPieces = new bool[adAquaPieces.Length];

        //テスト用
        for (int i = 0; i < avalableAdPieces.Length; i++)
        {
            avalableAdPieces[i] = true;
        }
    }
}
