using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static GameObject selected = null;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit.collider != null)
            {
                selected = hit.collider.gameObject;
            }
            else
            {
                selected = null;
            }
        }
    }
}
