//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script manages the placement, deletion and hovering of rooms, calling respective functions from Slot.cs

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; } //only allows other script to read the variable, not change it.
    //since only 1 Placement Manager is meant to exist at a time, this ensures every script can easily reference it.
    private GameObject _selectedRoomPrefab; //can be set by any of the room buttons, RoomButton.cs, via a method in this script.
    [SerializeField] private GameObject _roomSlot; //stores the empty room, also known as the room slot, to be instantiated when a room is deleted.
    private Slot _hoveredSlot; 
    private Room _hoveredRoom;
    //refers to a single room in a ship; is used in the functions of this script to represent the current room or slot the player is hovering over respectively.
    [SerializeField] private Ship _ship; //represents the player ship; only used in the Ship Design scene.
    [SerializeField] private TMP_Text _roomLimitText; //is used to tell the player why a certain action couldn't be done in the Ship Design scene.
    private Scene _currentScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } // Checks if another instance already exists and destroys this duplicate
        Instance = this;

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
        //bunch of safety precautions as the script needs everything here to be set properly in order to work.
    }

    void Update()
    {
        if (_currentScene.name == "Combat" && CombatManager.Instance.CombatActive == false) return;
        //since an object with this script is present in the Combat scene too, this checks if this script can perform its methods, based on if combat is active.
        OnHover();
        OnClick();
    }

    public void OnHover()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 
        //coverts pixels into world coordinates the starting point of the raycast.
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

        if (hit.collider != null) //checks if any collider was hit, using a raycast from the cursor's position.
        {
            Slot slot = hit.collider.GetComponent<Slot>(); //checks if the object has the Slot.cs script attached
            Room room = slot.GetComponent<Room>(); //checks if the object has the Room.cs script attached
            if (slot != null)
            {
                if (_hoveredSlot != slot) //checks if the mouse is on a new slot
                {
                    if (_hoveredSlot != null) _hoveredSlot.Highlight(false); //sets highlight to inactive for previous slot
                    if (_hoveredRoom != null) _hoveredRoom.Target(false); //sets the Target game object, used in room prefabs, to inactive for previous room, if any.

                    if (_hoveredRoom != null)
                    {
                        Weapon weapon = _hoveredRoom.GetComponent<Weapon>(); //checks if the object has the Weapon.cs script attached
                        if (weapon != null && weapon.TargetRoom != null) weapon.TargetRoom.Target(false);
                        //sets the Target game object, used in room prefabs, to inactive for the previous targeted room, if any.
                    }
                    //this 3 loops are if the player moves from one slot/room directly to another slot/room.

                    _hoveredSlot = slot;  
                    if (room != null) _hoveredRoom = room; //stores the new hovered slot

                    if (_currentScene.name == "Combat" && room != null && room.Ship.gameObject.name.Contains("Enemy")
                        && CombatManager.Instance.selectedWeapon != null) 
                    {
                        room.Target(true);
                        return;
                    }//checks if in the Combat scene, room is in the enemy ship and if the player has already selected a weapon and if everything is true, 
                    //that room is now set to be targeted by the weapon, which is selectedWeapon.

                    _hoveredSlot.Highlight(true); //sets highlight to active by calling a function in the Slot.cs
                    if (_currentScene.name == "Combat" && room != null && room.Ship.gameObject.name.Contains("Player"))
                    {
                        Weapon weapon = room.GetComponent<Weapon>();
                        if (weapon != null && weapon.TargetRoom != null) weapon.TargetRoom.Target(true);
                    } //if the player is currently hovering over one of their weapon rooms, and that weapon room has a targeted enemy room
                    // this loop gets the Target game object of the TargetRoom to appear, showing the player what room the weapon is targeting
                }
                return;
            }
        }
        if (_hoveredRoom != null && _currentScene.name == "Combat")
        {
            Weapon weapon = _hoveredRoom.GetComponent<Weapon>();
            if (weapon != null && weapon.TargetRoom != null) weapon.TargetRoom.Target(false);
        }
        if (_hoveredSlot != null)
        {
            _hoveredSlot.Highlight(false);
            _hoveredSlot = null;
        }
        if (_hoveredRoom != null)
        {
            _hoveredRoom.Target(false);
            _hoveredRoom = null;
        } //resets all previous variables in this method, except TargetRoom for any weapons; 
        //used when the player goes from one slot/room onto anything other than another slot/room
    }

    void OnClick()
    {
        if (_hoveredSlot == null || _ship == null || _roomLimitText == null) return; 
        //checks if any of the 3 variables are null, to prevent errors

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_selectedRoomPrefab == null) return;
            Room Room = _selectedRoomPrefab.GetComponent<Room>();
            if (Room.Limit != 0)
            {
                string Roomtype = _selectedRoomPrefab.name;
                if (CountRooms(Roomtype) == Room.Limit)
                {
                    _roomLimitText.text = $"Max limit of room type {Roomtype} reached \nLimit: {Room.Limit}";
                    StartCoroutine(ShowRoomText());
                    return;
                } //checks if a limit for the amount of that specific room type that can be in the player ship has been set
                //if there was it checks if the player has already hit the limit
                //if the limt was hit, this loop runs, showing the player a text of why the room wasn't placed for a short duration of time, _roomLimitText.
            }
            _hoveredSlot.PlaceRoom(_selectedRoomPrefab); 
            //calls the function for placing a room, based on what the current selected room from the room menu is, in Slot.cs.
        }

        if (Mouse.current.rightButton.wasPressedThisFrame) _hoveredSlot.DeleteRoom(_roomSlot); 
        //calls the function for deleting a room in Slot.cs and replaces the room with the empty room prefab, _roomSlot.
        
    }

    public void SetSelectedRoom(GameObject roomPrefab) //is called by RoomButton.cs to assign the _selectedRoomPrefab for when the player wants to place that room.
    {
        _selectedRoomPrefab = roomPrefab;
    }

    public int CountRooms(string prefab) //used to check how many of a specific room type is in the player ship right now
    {
        int count = 0;
        foreach (Room room in _ship.Rooms) if (room.Prefab.name == prefab) count++;
        return count;
    }

    public void UponNoRooms() //this method will be called if the player hasn't placed a single room in their ship, 
    //since this causes errors in combat if not stopped, this method prevents it
    {
        _roomLimitText.text = "Combat cannot start with 0 rooms in the ship!";
        StartCoroutine(ShowRoomText());
    }

    private IEnumerator ShowRoomText() //a coroutine to show text, _roomLimitText, for a short amount of time. 
    {
        _roomLimitText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _roomLimitText.gameObject.SetActive(false);
    } //code taken from chat gpt
}

//code taken from chat gpt and modified