using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _roomMenuButton;

    public void ToggleRoomMenu()
    {
        if(!_roomMenu.activeSelf)
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27f, 2f);
            _roomMenu.SetActive(true);
        }
        else
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27f, -6f);
            _roomMenu.SetActive(false);
        }
    }
}
