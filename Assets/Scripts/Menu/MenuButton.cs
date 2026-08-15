//this script controls when the room menu opens

using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _roomMenu, _roomMenuButton,  _startCombatButton;
    void Awake()
    {
        if (_roomMenu == null)
        {
            Debug.LogError($"{name}: Room menu has not been set.");
            return;
        }
        if (_roomMenuButton == null)
        {
            Debug.LogError($"{name}: Room menu has not been set.");
            return;
        }
        if (_startCombatButton == null)
        {
            Debug.LogError($"{name}: Room menu has not been set.");
            return;
        }
    }
    public void ToggleRoomMenu() //gets called when the button is pressed
    {
        if (_roomMenu == null || _roomMenuButton == null || _startCombatButton == null) return;
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
