using UnityEngine;

public class PlayerShipBuilder : MonoBehaviour
{
    [SerializeField] private Transform _roomsParent;

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
        SavingPlayerShip ship = ShipManager.Instance.playerShip;

        foreach(RoomData room in ship.rooms)
        {
            GameObject newRoom = Instantiate(room.prefab, _roomsParent);
            newRoom.transform.localPosition = room.position;
            newRoom.name = room.name;
            RemoveOccupiedSlot(room.position);
        }
    }

    private void RemoveOccupiedSlot(Vector3 position)
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