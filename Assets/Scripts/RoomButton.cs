using UnityEngine;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private GameObject roomMenu;
    [SerializeField] private GameObject roomMenuButton;

    public void ToggleRoomMenu()
    {
        if(!roomMenu.activeSelf)
        {
            roomMenuButton.transform.localPosition = new Vector2(-27f, 2f);
            roomMenu.SetActive(true);
        }
        else
        {
            roomMenuButton.transform.localPosition = new Vector2(-27f, -6f);
            roomMenu.SetActive(false);
        }
    }
}
