using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            _highlight.SetActive(true);
        }
        else
        {
            _highlight.SetActive(false);
        }
    }
}
