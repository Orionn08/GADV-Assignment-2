using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    public GameObject currentRoom;
    [SerializeField] private GameObject _roomSlotPrefab;
    [SerializeField] private GameObject _roomPlaceholderPrefab;

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

    void PlaceRoom()
    {
        var spawnedRoom = Instantiate(_roomPlaceholderPrefab, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        Destroy(gameObject);
    }

    void DeleteRoom()
    {
        var spawnedRoom = Instantiate(_roomSlotPrefab, transform.position, Quaternion.identity);
        spawnedRoom.name = $"Room {transform.position.x} {transform.position.y}";
        Destroy(gameObject);
    }    
}
