//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script rebuilds the player's ship layout in the Ship Design and Combat scenes.
//it works closely together with SavingPlayerShip.cs

using UnityEngine;

public class PlayerShipBuilder : MonoBehaviour
{
    [SerializeField] private Transform _roomsParent; //needed to instatiated all the saved rooms from SavingPlayerShip.cs into this transfrom.

    private void Start()
    {
        if (_roomsParent == null)
        {
            Debug.LogError($"{name}: Rooms' parent has not been set.");
            return;
        }
        BuildShip();
    }

    private void BuildShip()
    {
        SavingPlayerShip ship = GameManager.Instance.PlayerShip;
        if (ship.Rooms.Count == 0) return; //should never occur due to combat not being able to be started if there are no rooms in the player ship
        //and the fact that the room count doesnt change in the combat scene; only here for safety sake.
        
        foreach(RoomData room in ship.Rooms)
        {
            GameObject newRoom = Instantiate(room.prefab, _roomsParent);
            newRoom.transform.localPosition = room.position;
            newRoom.name = room.name;
            Room roomScript = newRoom.GetComponent<Room>();
            if (roomScript != null) roomScript.Prefab = room.prefab; //should not occur since only rooms can be saved; here for precaution.
            RemoveOccupiedSlot(room.position); //deletes the room slot prefab which has the same position as the saved room.
        } //instatiates every room according to what was saved in SavingPlayerShip.cs and what GameManager has stored.
    }

    private void RemoveOccupiedSlot(Vector3 position) //simplys deletes the room slot prefab, since it represents an empty room, 
    //when a room that was saved was placed at the same position; prevents doubled rooms or rooms stacked on one another.
    {
        foreach(Transform slot in _roomsParent)
        {
            if(slot.localPosition == position && slot.GetComponent<Slot>() != null)
            {
                Destroy(slot.gameObject);
                break;
            } 
        }
    }
}
//made with the help of Chat GPT