//this script creates all the room slots in a proper grid

using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height; //determines size of grid, can be changed in the inspector
    [SerializeField] private Transform _rooms; //for grouping all rooms before under an empty game object
    [SerializeField] private Slot _roomSlotPrefab; //the room slot, a prefab, to be instantiated later

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        if (_width <= 0)
        {
            Debug.LogError($"{name}: No width has been set or is negative");
            return;
        }
        if (_height <= 0)
        {
            Debug.LogError( $"{name}: No height has been set or is negative");
            return;
        }

        for(int x = 0; x < _width; x++) //creates x amount of rows according to _width
        {
            for(int y = 0; y < _height; y++) //creates x amount of columns according to _height
            {
                float xPos = x * 7f;
                float yPos = y * 4f; //ensures that each room slot appears right next to each other and also doesn't overlap

                var roomSlot = Instantiate(_roomSlotPrefab, _rooms);
                roomSlot.transform.localPosition = new Vector2(xPos, yPos);
                //creates each room slot at its respective positions under the _rooms game object
                roomSlot.name = $"Room ({roomSlot.transform.position.x} {roomSlot.transform.position.y})";
                //gives each room slot a name according to its coordinates
            }
        }
    }
}

//followed code from https://www.youtube.com/watch?v=kkAjpQAM-jE&t=371s
