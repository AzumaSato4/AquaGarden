using UnityEngine;

public class GalleryBoard : MonoBehaviour
{
    public GameObject[] galleryTiles;
    public GameObject[] startSpots;

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
}
