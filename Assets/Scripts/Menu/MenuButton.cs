//this script controls when the room menu opens

using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _roomMenuButton;

    public void ToggleRoomMenu() //gets called when the button is pressed
    {
        if(!_roomMenu.activeSelf) //sets the room menu to the opposite state of what it was when the button was pressed
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27f, 2f); 
            //moves the room menu button upwards so it doesnt block the room menu
            _roomMenu.SetActive(true);
        }
        else
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27f, -6f);
            _roomMenu.SetActive(false);
        }
    }
}
