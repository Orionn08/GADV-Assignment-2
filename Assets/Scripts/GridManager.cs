using UnityEngine;
using UnityEngine.LightTransport.PostProcessing;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Transform _rooms;
    [SerializeField] private Room _roomSlotPrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                float xPos = x * 7f;
                float yPos = y * 4f;

                var RoomSlot = Instantiate(_roomSlotPrefab, new Vector2(xPos,yPos), Quaternion.identity, _rooms);
                RoomSlot.name = $"Room {RoomSlot.transform.position.x} {RoomSlot.transform.position.y}";
            }
        }
    }
}

//followed code from https://www.youtube.com/watch?v=kkAjpQAM-jE&t=371s
