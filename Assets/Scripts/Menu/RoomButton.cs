//this script contains the function to set itself up
//it also contains the function for when its clicked

using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }
    public void Setup(GameObject prefab, Sprite icon, string name)
    {
        roomPrefab = prefab;
        roomName = name;
        //sets the prefab and name to their respective object when the function is called

        button = GetComponent<Button>();

        iconImage.sprite = icon; //ensures that each room button can be clearly identified
        roomNameText.text = name; //gives each button a name, according to the prefab its linked to

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("Selected: " + roomName);
        _placementManager.SetSelectedRoom(roomPrefab); 
        //sets the prefab linked to the button that was click to be selected room
    }
}

//used chat gpt to help fix the script