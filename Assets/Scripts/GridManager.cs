using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private RoomSlot _roomSlotPrefab;

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

                var RoomSlot = Instantiate(_roomSlotPrefab, new Vector3(xPos,yPos,0f), Quaternion.identity);
                RoomSlot.name = $"Room {RoomSlot.transform.position.x} {RoomSlot.transform.position.y}";
            }
        }
    }
}

//followed code from https://www.youtube.com/watch?v=kkAjpQAM-jE&t=371s
