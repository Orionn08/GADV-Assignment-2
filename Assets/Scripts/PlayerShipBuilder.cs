using UnityEngine;

public class PlayerShipBuilder : MonoBehaviour
{
    [SerializeField] private Transform roomsParent;

    private void Start()
    {
        BuildShip();
    }

    private void BuildShip()
    {
        PlayerShip ship = ShipManager.Instance.playerShip;

        foreach(RoomData room in ship.rooms)
        {
            GameObject newRoom = Instantiate(room.prefab, roomsParent);
            newRoom.transform.localPosition = room.position;
            newRoom.name = room.name;
            RemoveOccupiedSlot(room.position);
        }
    }

    private void RemoveOccupiedSlot(Vector3 position)
    {
        foreach(Transform slot in roomsParent)
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