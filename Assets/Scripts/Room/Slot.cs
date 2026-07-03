using UnityEngine;
public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
<<<<<<< HEAD:Assets/Scripts/Room/Slot.cs
=======


    //asked ChatGPT for code and editied as i saw fit
>>>>>>> 5ca405275fcd418b9430db13f9174ed07801ebf1:Assets/Scripts/Room.cs
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

<<<<<<< HEAD:Assets/Scripts/Room/Slot.cs
    public void DeleteRoom(GameObject roomSlot)
    {
        var spawnedRoom = Instantiate(roomSlot, transform.position, Quaternion.identity);
=======
    public void DeleteRoom(GameObject roomslot)
    {
        var spawnedRoom = Instantiate(roomslot, transform.position, Quaternion.identity);
>>>>>>> 5ca405275fcd418b9430db13f9174ed07801ebf1:Assets/Scripts/Room.cs
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        spawnedRoom.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }    
}
