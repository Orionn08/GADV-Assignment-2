//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script contains the functions that will be called by PlacementManager.cs; doesn't actually call any of its methods itself.
//all rooms have a Slot.cs script, meaning the different room type prefabs all have this script.

using UnityEngine;
public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight; //a game object that can be set to active by PlacementManager based on situation, mostly for when hovered over.
    private Ship ship; //assigns the ship of the slot/room.

    private void Awake()
    {
        ship = GetComponentInParent<Ship>();

        if (_highlight == null)
        {
            Debug.LogError($"{name}: Highlight has not been set.");
            return;
        }
    }

    public void Highlight(bool state) //sets the highlight object to the opposite state it was originally at
    {
        _highlight.SetActive(state); //turns on and off highlight so the player can see what room they are hovering over
    }
    
    public void PlaceRoom(GameObject selectedRoom)
    {
        var spawnedRoom = Instantiate(selectedRoom, transform.position, Quaternion.identity, gameObject.transform.parent); 
        //spawns selected room at the same position as the room which this script is attached to
        spawnedRoom.name = $"{selectedRoom.name} ({transform.position.x} {transform.position.y})";
        //gives the spawned room a name of the selected room plus its coordinates
        Room room = spawnedRoom.GetComponent<Room>();

    if(room != null) room.Prefab = selectedRoom; //assigns selectedRoom to the Prefab variable of the room that was instantiated
    
    else Debug.LogError($"{name}: Spawned room does not have Room.cs attached"); //a safety precaution, since this should not happen
    
        Room deletedRoom = gameObject.GetComponent<Room>();
        if (deletedRoom != null) ship.RemoveRoom(deletedRoom); //removes the room from the Rooms list, in the Ship.cs script, which stores all rooms in the ship.
        
        Destroy(gameObject);
        ship.AddRoom(room); //adds the new instantiated room to the Rooms list, in the Ship.cs script.
    }

    public void DeleteRoom(GameObject roomSlot)
    {
        var spawnedRoom = Instantiate(roomSlot, transform.position, Quaternion.identity, gameObject.transform.parent);
        //spawns the room slot prefab, indicating the room is now empty.
        spawnedRoom.name = $"Room ({transform.position.x} {transform.position.y})"; 
        //keeps the same naming convention of naming by the coordinates of the room.
        Room room = GetComponent<Room>();
        if (room != null) ship.RemoveRoom(room);
        Destroy(gameObject); //deletes this current room.
    }
}