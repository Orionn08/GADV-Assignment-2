using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomButtonPrefab;
    [SerializeField] private Transform _roomsDisplay;
    [SerializeField] private GameObject[] _roomPrefabs;
    [SerializeField] private string[] _roomNames;
    [SerializeField] private Sprite[] _roomIcons;

    void Start()
    {
        for (int i = 0; i < _roomPrefabs.Length; i++)
        {
            GameObject buttonObj = Instantiate(_roomButtonPrefab, _roomsDisplay);
            buttonObj.name = _roomNames[i] + " Button";

            RoomButton button = buttonObj.GetComponent<RoomButton>();
            button.Setup(_roomPrefabs[i], _roomIcons[i], _roomNames[i]);
        }
    }
} 

//used chat gpt to help fix the script
