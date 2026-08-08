//this script contains the functions that will be called by PlacementManager.cs

using UnityEngine;
public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    private Ship ship;

    private void Awake()
    {
        ship = GetComponentInParent<Ship>();
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

    if(room != null)
    {
        room.prefab = selectedRoom;
    }
    else
    {
        Debug.LogError($"{name}: Spawned room does not have Room.cs attached");
    }
        Room deletedRoom = gameObject.GetComponent<Room>();
        if (deletedRoom != null) ship.RemoveRoom(deletedRoom);
        
        Destroy(gameObject); //deletes this current room
        ship.AddRoom(room);
    }

    public void DeleteRoom(GameObject roomSlot)
    {
        var spawnedRoom = Instantiate(roomSlot, transform.position, Quaternion.identity, gameObject.transform.parent);
        //spawns the room slot prefab, indicating the room is now empty
        spawnedRoom.name = $"Room ({transform.position.x} {transform.position.y})"; 
        //keeps the same naming convention of naming by the coordinates of the room
        Room room = GetComponent<Room>();
        ship.RemoveRoom(room);
        Destroy(gameObject); //deletes this current room
    }
}