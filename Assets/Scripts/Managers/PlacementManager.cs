//this script manages the placement of rooms and calls respective functions from Slot.cs

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedRoomPrefab;
    [SerializeField] private GameObject _roomSlot;
    private Slot _hoveredSlot; //refers to a single room in the ship
    [SerializeField] private Ship _ship;
    [SerializeField] private TMP_Text _roomLimitText;

    [SerializeField] private float _maxBridge;
    [SerializeField] private float _maxEngine;
    [SerializeField] private float _maxLaserGun;
    [SerializeField] private float _maxMachineGun;
    [SerializeField] private float _maxMissileLauncher;
    [SerializeField] private float _maxReactor;
    [SerializeField] private float _maxShieldGenerator;

    private void Awake()
    {
        if (_roomSlot == null)
        {
            Debug.LogError($"{name}: Room slot prefab has not been set.");
            return;
        }
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
        if (_hoveredSlot == null) return;
        if (_selectedRoomPrefab == null) return; //if either variable is null then the function doesn't run to prevent errors
        if (_ship == null) return;
        if (_roomLimitText == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_maxBridge != 0 && _selectedRoomPrefab.name == "Bridge" && CountRooms("Bridge") == _maxBridge)
            {
                _roomLimitText.text = $"Max limit of room type Bridge reached \nLimit: {_maxBridge}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxEngine != 0 && _selectedRoomPrefab.name == "Engine" && CountRooms("Engine") == _maxEngine)
            {
                _roomLimitText.text = $"Max limit of room type Engine reached \nLimit: {_maxEngine}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxLaserGun != 0 && _selectedRoomPrefab.name == "Laser Gun" && CountRooms("Laser Gun") == _maxLaserGun)
            {
                _roomLimitText.text = $"Max limit of room type Engine reached \nLimit: {_maxLaserGun}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxMachineGun != 0 && _selectedRoomPrefab.name == "Machine Gun" && CountRooms("Machine Gun") == _maxMachineGun)
            {
                _roomLimitText.text = $"Max limit of room type Engine reached \nLimit: {_maxMachineGun}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxMissileLauncher != 0 && _selectedRoomPrefab.name == "Missile Launcher" && CountRooms("Missile Launcher") == _maxMissileLauncher)
            {
                _roomLimitText.text = $"Max limit of room type Engine reached \nLimit: {_maxMissileLauncher}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxReactor != 0 && _selectedRoomPrefab.name == "Reactor" && CountRooms("Reactor") == _maxReactor)
            {
                _roomLimitText.text = $"Max limit of room type Reactor reached \nLimit: {_maxReactor}";
                StartCoroutine(ShowRoomText());
                return;
            }
            else if (_maxShieldGenerator != 0 && _selectedRoomPrefab.name == "Shield Generator" && CountRooms("Shield Generator") == _maxShieldGenerator)
            {
                _roomLimitText.text = $"Max limit of room type Shield Generator reached \nLimit: {_maxShieldGenerator}";
                StartCoroutine(ShowRoomText());
                return;
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