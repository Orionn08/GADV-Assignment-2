//this script manages the placement of rooms and calls respective functions from room.cs

using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedRoomPrefab;
    [SerializeField] private GameObject _roomSlot;
    private Camera _cam;
    private Slot _hoveredSlot;

    private void Awake()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        OnHover();
        OnClick();
    }

    void OnHover()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            Slot slot = hit.collider.GetComponent<Slot>();

            if (slot != null)
            {
                if (_hoveredSlot != slot)
                {
                    if (_hoveredSlot != null)
                        _hoveredSlot.Highlight(false);

                    _hoveredSlot = slot;
                    _hoveredSlot.Highlight(true);
                }
                return;
            }
        }
        if (_hoveredSlot != null)
        {
            _hoveredSlot.Highlight(false);
            _hoveredSlot = null;
        }
    }

    void OnClick()
    {
        if (_hoveredSlot == null) return;
        if (_selectedRoomPrefab == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _hoveredSlot.PlaceRoom(_selectedRoomPrefab);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _hoveredSlot.DeleteRoom(_roomSlot);
        }
    }

    public void SetSelectedRoom(GameObject roomPrefab)
    {
        _selectedRoomPrefab = roomPrefab;
    }
}

//code taken from chat gpt and modified 