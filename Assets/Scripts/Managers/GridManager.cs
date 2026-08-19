//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script creates all the room slots in a proper grid

using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height; //determines size of grid, can be changed in the inspector
    [SerializeField] private Transform _rooms; //for grouping all rooms before under an empty game object
    //this will be necessary in later parts of the game too, like saving the rooms of the ship, or the Ship.cs script knowing every single room under it.
    [SerializeField] private Slot _roomSlotPrefab; //the room slot, a prefab, to be instantiated later

    void Awake()
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
        //safety precaution in case either neccessary varible is negative or wasn't set, since GenerateGrid won't work without them.

        GenerateGrid();
    }

    void GenerateGrid()
    {   
        for(int x = 0; x < _width; x++) //creates x amount of rows according to _width.
        {
            for(int y = 0; y < _height; y++) //creates x amount of columns according to _height.
            {
                float xPos = (x * 7) -3;
                float yPos = (y * 4) -3; 
                //ensures that each room slot appears right next to, above or below to each other, doesn't overlap and keeps the grid looking tidy.

                var roomSlot = Instantiate(_roomSlotPrefab, _rooms);
                roomSlot.transform.localPosition = new Vector2(xPos, yPos);
                //creates each room slot at its respective positions under the _rooms game object.
                roomSlot.name = $"Room Slot ({roomSlot.transform.position.x} {roomSlot.transform.position.y})";
                //gives each room slot a name according to its coordinates so it can be identified easily.
            }
        }
    }
}

//followed code from https://www.youtube.com/watch?v=kkAjpQAM-jE&t=371s
