using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _roomSlot;
    [SerializeField] private GameObject _selectedRoom;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            _highlight.SetActive(true);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceRoom();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                DeleteRoom();
            }
        }
        else
        {
            _highlight.SetActive(false);
        }
    }
    //asked ChatGPT for code and editied as i saw fit

    void PlaceRoom()
    {
        var spawnedRoom = Instantiate(_selectedRoom, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {_selectedRoom} {transform.position.x} {transform.position.y}";
        Destroy(gameObject);
    }

    void DeleteRoom()
    {
        var spawnedRoom = Instantiate(_roomSlot, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        Destroy(gameObject);
    }    
}
