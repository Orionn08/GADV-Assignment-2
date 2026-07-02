using UnityEngine;
using UnityEngine.InputSystem;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _roomSlot;
    [SerializeField] private Transform _rooms;


    //asked ChatGPT for code and editied as i saw fit
    public void Highlight(bool state)
    {
        _highlight.SetActive(state);
    }
    public void PlaceRoom(GameObject selectedRoom)
    {
        var spawnedRoom = Instantiate(selectedRoom, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {selectedRoom} {transform.position.x} {transform.position.y}";
        spawnedRoom.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }

    public void DeleteRoom()
    {
        var spawnedRoom = Instantiate(_roomSlot, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        spawnedRoom.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }    
}
