//this script manages the placement of rooms and calls respective functions from Slot.cs

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedRoomPrefab;
    [SerializeField] private GameObject _roomSlot;
    private Slot _hoveredSlot; //refers to a single room in the ship
    [SerializeField] private Ship _ship;
    [SerializeField] private TMP_Text _roomLimitText;
    private Scene _currentScene;

    private void Awake()
    {
        if (_roomSlot == null)
        {
            Debug.LogError($"{name}: Room slot prefab has not been set.");
            return;
        }
        _currentScene = SceneManager.GetActiveScene();
        if (_currentScene.name == "Ship Design")
        {
            if (_ship == null)
            {
                Debug.LogError($"{name}: Ship has not been set.");
                return;
            }
            if (_roomLimitText == null)
            {
                Debug.LogError($"{name}: Room limit text has not been set.");
                return;
            }
        }
    }

    void Update()
    {
        OnHover();
        OnClick();
    }

    public void OnHover()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 
        //coverts pixels into world coordinates the starting point of the raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

        if (hit.collider != null) //checks if anything was hit
        {
            Slot slot = hit.collider.GetComponent<Slot>(); //checks if the object has the slot.cs script attached

            if (slot != null)
            {
                if (_hoveredSlot != slot) //checks if the mouse is on a new slot
                {
                    if (_hoveredSlot != null)
                        _hoveredSlot.Highlight(false); //sets highlight to inactive for previous slot

                    _hoveredSlot = slot;  //stores new slot
                    _hoveredSlot.Highlight(true); //sets highlight to active
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
        if (_hoveredSlot == null) return; //if either variable is null then the function doesn't run to prevent errors
        if (_ship == null) return;
        if (_roomLimitText == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_selectedRoomPrefab == null) return;
            Room Room = _selectedRoomPrefab.GetComponent<Room>();
            if (Room.limit != 0)
            {
                string Roomtype = _selectedRoomPrefab.name;
                if (CountRooms(Roomtype) == Room.limit)
                {
                    _roomLimitText.text = $"Max limit of room type {Roomtype} reached \nLimit: {Room.limit}";
                    StartCoroutine(ShowRoomText());
                    return;
                }
            }
            
            _hoveredSlot.PlaceRoom(_selectedRoomPrefab); //calls the function for placing a room in Slot.cs
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _hoveredSlot.DeleteRoom(_roomSlot); //calls the function for deleting a room in Slot.cs
        }
    }

    public void SetSelectedRoom(GameObject roomPrefab) //is called by RoomButton.cs
    {
        _selectedRoomPrefab = roomPrefab;
    }

    public int CountRooms(string prefab)
    {
        int count = 0;
        foreach (Room room in _ship._rooms) if (room.prefab.name == prefab) count++;
        return count;
    }

    private IEnumerator ShowRoomText()
    {
        _roomLimitText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _roomLimitText.gameObject.SetActive(false);
    }
}

//code taken from chat gpt and modified