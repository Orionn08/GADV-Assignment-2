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
        }
    }
}