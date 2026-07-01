using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomSlot;
    [SerializeField] private GameObject _selectedRoom;
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null)
            return;

        RoomSlot slot = hit.collider.GetComponent<RoomSlot>();

        if (slot == null)
            return;

        slot.Highlight();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            slot.PlaceRoom(_selectedRoom);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            slot.DeleteRoom();
        }
    }
}
