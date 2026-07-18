//this script controls when the room menu opens

using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _roomMenuButton;
    [SerializeField] private GameObject _startCombatButton;

    public void ToggleRoomMenu() //gets called when the button is pressed
    {
        if(!_roomMenu.activeSelf) //sets the room menu to the opposite state of what it was when the button was pressed
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27, 2);
            _startCombatButton.transform.localPosition = new Vector2(27, 2);
            //moves the room menu button upwards so it doesnt block the room menu
            _roomMenu.SetActive(true);
        }
        else
        {
            _roomMenuButton.transform.localPosition = new Vector2(-27, -6);
            _startCombatButton.transform.localPosition = new Vector2(27, -6);
            _roomMenu.SetActive(false);
        }
    }
}
