//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script controls when the room menu opens and the movement of the 2 buttons in the Ship Design scene

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
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.
    }
    public void ToggleRoomMenu() //gets called when the button is pressed
    {
        if (_roomMenu == null || _roomMenuButton == null || _startCombatButton == null) return; 
        //checks if any necessary varaibles are null since this function needs all of them.
        
        if(!_roomMenu.activeSelf) //sets the room menu to the opposite state of what it was when the button was pressed
        {
            _roomMenuButton.transform.localPosition = new Vector2(-29, 5);
            _startCombatButton.transform.localPosition = new Vector2(29, 5);
            //moves both the Room Menu and Start Combat buttons upwards so they doesnt block the room menu.
            _roomMenu.SetActive(true);
        }
        else
        {
            _roomMenuButton.transform.localPosition = new Vector2(-29, -6);
            _startCombatButton.transform.localPosition = new Vector2(29, -6);
            //resets both the Room Menu and Start Combat buttons back to their original positions.
            _roomMenu.SetActive(false);
        }
    }
}
