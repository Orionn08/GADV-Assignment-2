using System.Threading;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomButtonPrefab;
    [SerializeField] private Transform _rooms;
    [SerializeField] private GameObject[] _roomPrefabs;
    [SerializeField] private Sprite[] _roomIcons;

    void Start()
    {
        for (int i = 0; i < _roomPrefabs.Length; i++)
        {
            GameObject buttonObj = Instantiate(_roomButtonPrefab, _rooms);

            RoomButton button = buttonObj.GetComponent<RoomButton>();
            button.Setup(_roomPrefabs[i], _roomIcons[i]);
        }
    }
}
