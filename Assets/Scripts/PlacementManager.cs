using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;
    [SerializeField] private GameObject selectedRoomPrefab;
    private Camera cam;
    private Room hoveredSlot;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    void Update()
    {
        OnHover();
        OnClick();
    }

    void OnHover()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            Room slot = hit.collider.GetComponent<Room>();

            if (slot != null)
            {
                if (hoveredSlot != slot)
                {
                    if (hoveredSlot != null)
                        hoveredSlot.Highlight(false);

                    hoveredSlot = slot;
                    hoveredSlot.Highlight(true);
                }
                return;
            }
        }
        if (hoveredSlot != null)
        {
            hoveredSlot.Highlight(false);
            hoveredSlot = null;
        }
    }

    void OnClick()
    {
        if (hoveredSlot == null) return;
        if (selectedRoomPrefab == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            hoveredSlot.PlaceRoom(selectedRoomPrefab);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            hoveredSlot.DeleteRoom();
        }
    }

    public void SetSelectedRoom(GameObject roomPrefab)
    {
        selectedRoomPrefab = roomPrefab;
    }
}

//code taken from chat gpt and modified 