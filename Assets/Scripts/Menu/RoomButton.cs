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

        
        if (Weapon != null)
        {
            roomNameText.text += " (Weapon)";
            roomNameText.text += $"\nHealth: {Room._maxHealth}";
            roomNameText.text += $"\nCooldown: {Weapon.cooldown}s \nDamage: {Weapon.damage}";
        }
        if (Shield != null) 
        {
            roomNameText.text += " (Defense)";
            roomNameText.text += $"\nHealth: {Room._maxHealth}";
            roomNameText.text += $"\nCooldown: {Shield.cooldown}s";
        }
        if (prefab.name == "Bridge")
        {
            roomNameText.text += $" (Support)";
            roomNameText.text += $"\nHealth: {Room._maxHealth}";
            roomNameText.text += $"\n Boosts adjacent rooms health";
        } 
        if (prefab.name == "Engine")
        {
            roomNameText.text += $" (Support)";
            roomNameText.text += $"\nHealth: {Room._maxHealth}";
            roomNameText.text += $"\nDecreases adjacent rooms' cooldowns";
        }
        if (prefab.name == "Reactor")
        {
            roomNameText.text += $" (Support)";
            roomNameText.text += $"\nHealth: {Room._maxHealth}";
            roomNameText.text += $"\n Increases damage of adjacent weapons";
        }
        

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (_placementManager == null) return;
        _placementManager.SetSelectedRoom(roomPrefab); 
        //sets the prefab linked to the button that was click to be selected room
    }
}

//used chat gpt to help fix the script