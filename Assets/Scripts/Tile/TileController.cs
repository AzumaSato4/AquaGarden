using UnityEngine;

public class TileController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.enabled = true;
    }

    private void OnMouseExit()
    {
        spriteRenderer.enabled = false;
    }
}
