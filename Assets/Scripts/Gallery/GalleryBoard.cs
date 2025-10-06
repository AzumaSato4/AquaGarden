using UnityEngine;

public class GalleryBoard : MonoBehaviour
{
    public GameObject[] galleryTiles;
    public GameObject[] startSpots;
    public GameObject[] roundSpots;

    public bool[] isPlayer;

    private void Awake()
    {
        isPlayer = new bool[23];

        for (int i = 0; i < 23; i++)
        {
            isPlayer[i] = false;
        }
    }

    private void Start()
    {
        foreach (GameObject spot in startSpots)
        {
            spot.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public void ResetTile()
    {
        for (int i = 0;i < galleryTiles.Length;i++)
        {
            galleryTiles[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        for (int i = 0; i < startSpots.Length;i++)
        {
            startSpots[i].GetComponent <CircleCollider2D>().enabled = false;
        }
    }
}
