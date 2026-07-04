using UnityEngine;
public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    public void Highlight(bool state)
    {
        _highlight.SetActive(state);
    }
    
    public void PlaceRoom(GameObject selectedRoom)
    {
        var spawnedRoom = Instantiate(selectedRoom, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {selectedRoom.name} {transform.position.x} {transform.position.y}";
        spawnedRoom.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }

    public void DeleteRoom(GameObject roomSlot)
    {
        var spawnedRoom = Instantiate(roomSlot, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        spawnedRoom.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }    
}
