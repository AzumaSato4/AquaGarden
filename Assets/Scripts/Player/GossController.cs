using UnityEngine;

public class GossController : MonoBehaviour
{
    public GossManager gossManager;

    int index;
    GameObject selected;

    public void MoveStart(int index, GameObject selcted)
    {
        this.index = index;
        this.selected = selcted;
        Invoke("MoveGoss", 1.0f);
    }

    public void MoveGoss()
    {
        gossManager.MoveGallery(index, selected.name);
        transform.position = selected.transform.position;
        if (selected.GetComponent<BoxCollider2D>() != null)
            selected.GetComponent<BoxCollider2D>().enabled = false;
        if (selected.GetComponent<CircleCollider2D>() != null)
            selected.GetComponent<CircleCollider2D>().enabled = false;
    }
}
