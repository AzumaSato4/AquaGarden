using UnityEngine;

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

        Debug.Log("広告カード" + spotNum + "番を選択");
        adCard.transform.localScale = defSize;

        StartAd();
    }

    void StartAd()
    {
        adMoney = 0;
        switch (adData.adID)
        {
            case 1: //SmallFish
                SelectPairAd();
                break;
            case 2: //LargeFish
                SelectSoloAd();
                break;
            case 3: //Shark + Coral
                SelectAd();
                break;
            case 4: //SeaTurtle + Coral
                SelectAd();
                break;
        }
    }

    void SelectSoloAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        string nameA = adData.nameA;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                string name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
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

        current.EndAd();
    }

    void SelectPairAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        string nameA = adData.nameA;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                string name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
                if (name == nameA)
                {
                    Debug.Log("発見A");
                    countA++;
                }
            }

            while (countA >= 2)
            {
                adMoney++;
                Debug.Log(adMoney + "資金獲得");

                countA -= 2;
            }
        }

        current.money += adMoney;
        Debug.Log("合計獲得資金" + adMoney);

        current.EndAd();
    }

    void SelectAd()
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        string nameA = adData.nameA;
        string nameB = adData.nameB;

        for (int i = 0; i < 6; i++)
        {
            int countA = 0;
            int countB = 0;

            foreach (GameObject piece in current.aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().slotPieces)
            {
                string name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
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

        current.EndAd();
    }
}
