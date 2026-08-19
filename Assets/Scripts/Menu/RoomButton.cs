//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script contains the function to set itself up
//it also contains the function for when its clicked

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RoomButton : MonoBehaviour
{   
    private GameObject roomPrefab;
    private string roomName;
    private Button button;
    //information for the button, will be assigned in the Setup function when it gets called

    [SerializeField] private PlacementManager _placementManager; 
    //sets the variable to be the placement manager so functions can be called later
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text roomNameText;

    void Awake()
    {
        _placementManager = FindFirstObjectByType<PlacementManager>();

        if (_placementManager == null)
        {
            Debug.LogError($"{name}: Placement manager can't be found.");
            return;
        }
    }
    public void Setup(GameObject prefab, Sprite icon, string name)
    {
        roomPrefab = prefab;
        roomName = name;
        //sets the prefab and name to their respective object when the function is called

        button = GetComponent<Button>();

        iconImage.sprite = icon; //ensures that each room button can be clearly identified
        roomNameText.text = name; //gives each button a name, according to the prefab its linked to

        Room Room = prefab.GetComponent<Room>();
        Weapon Weapon = prefab.GetComponent<Weapon>();
        Shield Shield = prefab.GetComponent<Shield>();
        //used to take information from the respective scripts to display in the Rooms Menu.

        if (Weapon != null)
        {
            roomNameText.text += " (Weapon)"; //shows the class of the room type
            roomNameText.text += $"\nHealth: {Room.MaxHealth}"; //shows the Max Health of the room type, without additional health which can be given by Bridge, a support room.
            roomNameText.text += $"\nCooldown: {Weapon.Cooldown}s \nDamage: {Weapon.Damage}"; 
            //states the damage and cooldown of the weapon.
        }
        if (Shield != null) 
        {
            roomNameText.text += " (Defense)"; //shows the class of the room type
            roomNameText.text += $"\nHealth: {Room.MaxHealth}";
            roomNameText.text += $"\nCooldown: {Shield.Cooldown}s";
            //states the cooldown of the sheild.
        }
        if (prefab.name == "Bridge")
        {
            roomNameText.text += $" (Support)"; //shows the class of the room type
            roomNameText.text += $"\nHealth: {Room.MaxHealth}";
            roomNameText.text += $"\n Boosts adjacent rooms health"; //tells the player the effect of this support room
        } 
        if (prefab.name == "Engine")
        {
            roomNameText.text += $" (Support)";
            roomNameText.text += $"\nHealth: {Room.MaxHealth}";
            roomNameText.text += $"\nDecreases adjacent rooms' cooldowns"; //tells the player the effect of this support room
        }
        if (prefab.name == "Reactor")
        {
            roomNameText.text += $" (Support)";
            roomNameText.text += $"\nHealth: {Room.MaxHealth}";
            roomNameText.text += $"\n Increases damage of adjacent weapons"; //tells the player the effect of this support room
        }
        //all of this information is displayed to help the player to understand the functions of each room.
        //it can also help the player better plan for how they want to set up their ship for the upcoming combat.

        button.onClick.AddListener(OnClick); //allows the button to be clicked
    }

    private void OnClick()
    {
        if (_placementManager == null) return; //safety precaution in case doesn't exist for some reason.
        _placementManager.SetSelectedRoom(roomPrefab); 
        //sets the prefab linked to the button that was clicked to be selected room, allowing it to then be instatiated by PlacementManager.
    }
}

//used chat gpt to help fix the script