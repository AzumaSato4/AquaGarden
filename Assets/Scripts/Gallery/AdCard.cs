using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class AdCard : MonoBehaviour
{
    public AdCardData adData;
    [SerializeField] int spotNum;

    [SerializeField] GameObject adCardPrefab;
    GameObject adCard;
    Vector2 defSize = new Vector2(2.0f, 2.0f);

    int adMoney = 0;

    private void Start()
    {
        adCard = Instantiate(adCardPrefab, transform.position, Quaternion.identity);
        adData = GameManager.instance.GetAdCardData(spotNum - 1);
        adCard.GetComponent<SpriteRenderer>().sprite = adData.sprite;
    }

    private void OnMouseEnter()
    {
        if (PhaseManager.currentPhase != PhaseManager.Phase.ad) return;
        adCard.transform.localScale = new Vector2 (2.5f, 2.5f);
    }

    private void OnMouseExit()
    {
        if (PhaseManager.currentPhase != PhaseManager.Phase.ad) return;
        adCard.transform.localScale = defSize;
    }

    private void OnMouseDown()
    {
        if (PhaseManager.currentPhase != PhaseManager.Phase.ad) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;
        Debug.Log("広告カード" + spotNum + "番を選択");
        adCard.transform.localScale = defSize;

        StartAd();
    }

    void StartAd()
    {
        adMoney = 0;
        switch (adData.adType)
        {
            case AdCardData.AdType.pair: //ペア
                SelectPairAd();
                break;
            case AdCardData.AdType.solo: //ソロ
                SelectSoloAd();
                break;
            case AdCardData.AdType.count: //個数
                SelectAd();
                break;
        }
    }

    void SelectSoloAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        PieceData.PieceName nameA = adData.nameA;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
                if (name == nameA)
                {
                    Debug.Log("発見A");
                    countA++;
                }
            }
            adMoney += countA;
        }

        current.money += adMoney;
        Debug.Log("合計獲得資金" + adMoney);
        ShowMessage("合計獲得資金" + adMoney);

        StartCoroutine(EndAdCoroutine(current));
    }

    void SelectPairAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        PieceData.PieceName nameA = adData.nameA;
        PieceData.PieceName nameB = adData.nameB;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;
            int countB = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
                if (name == nameA)
                {
                    Debug.Log("発見A");
                    countA++;
                }
                if (name == nameB)
                {
                    Debug.Log("発見B");
                    countB++;
                }
            }

            int count = countA + countB;

            while (count >= 2)
            {
                adMoney++;
                Debug.Log(adMoney + "資金獲得");

                count -= 2;
            }
        }

        current.money += adMoney;
        Debug.Log("合計獲得資金" + adMoney);
        ShowMessage("合計獲得資金" + adMoney);

        StartCoroutine(EndAdCoroutine(current));
    }

    void SelectAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        PieceData.PieceName nameA = adData.nameA;
        PieceData.PieceName nameB = adData.nameB;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;
            int countB = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
                if (name == nameA)
                {
                    Debug.Log("発見A");
                    countA++;
                }
                if (name == nameB)
                {
                    Debug.Log("発見B");
                    countB++;
                }
            }

            adMoney += (countA + countB);
        }

        current.money += adMoney;
        Debug.Log("合計獲得資金" + adMoney);
        ShowMessage("合計獲得資金" + adMoney);

        StartCoroutine(EndAdCoroutine(current));
    }

    IEnumerator EndAdCoroutine(PlayerManager current)
    {
        yield return new WaitForSeconds(2.0f);
        current.EndAd();
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
