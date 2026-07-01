using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomButton : MonoBehaviour
{
    private GameObject roomPrefab;
    private string roomName;

    private Button button;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text roomNameText;

    public void Setup(GameObject prefab, Sprite icon, string name)
    {
        roomPrefab = prefab;
        roomName = name;

        button = GetComponent<Button>();

        iconImage.sprite = icon;
        roomNameText.text = name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("Selected: " + roomName);

        // Later:
        // RoomPlacement.Instance.SelectRoom(roomPrefab);
    }
}

//used chat gpt to help fix the script