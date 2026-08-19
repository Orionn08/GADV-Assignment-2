//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script creates the room buttons using a prefab, it stores some of the information needed to be shown to the player;
//grabs the other necessary information from the prefab itself to show to the player.

using UnityEngine;

public class RoomButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomButtonPrefab; //the structure of the room buttons
    [SerializeField] private Transform _roomsDisplay; //parent object of the buttons
    [SerializeField] private GameObject[] _roomPrefabs;
    [SerializeField] private string[] _roomNames;
    [SerializeField] private Sprite[] _roomIcons;
    //lists to store the information of the different rooms so they can be instantiated properly

    void Start()
    {
        if (_roomButtonPrefab == null)
        {
            Debug.LogError("Room Button structure has not been set.");
            return;
        }
        if (_roomsDisplay == null)
        {
            Debug.LogError("Room buttons' parent has not been set.");
            return;
        }
        if (_roomIcons.Length != _roomNames.Length || _roomIcons.Length != _roomPrefabs.Length)
        {
            Debug.LogError("Room information lists aren't equal length.");
            return;
        }
        if (_roomIcons.Length == 0 || _roomNames.Length == 0 || _roomPrefabs.Length == 0)
        {
            Debug.LogError("One or more of the room information lists have no values.");
            return;
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.

        for (int i = 0; i < _roomPrefabs.Length; i++) //creates x amount of buttons, according to the length of the lists above
        {
            GameObject buttonObj = Instantiate(_roomButtonPrefab, _roomsDisplay); //creates room button under the _roomsDisplay game object
            buttonObj.name = _roomNames[i] + " Button"; //gives the room button a name according to the room it represents

            RoomButton button = buttonObj.GetComponent<RoomButton>(); 
            button.Setup(_roomPrefabs[i], _roomIcons[i], _roomNames[i]); //calls the function in RoomButton.cs to set up the button
        }
    }
} 
//used chat gpt to help fix the script
