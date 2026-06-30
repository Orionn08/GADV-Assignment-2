using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
     private GameObject _roomPrefab;
    private Button _button;
    [SerializeField] private Image iconImage;

    public void Setup(GameObject prefab, Sprite icon)
    {
        _button = GetComponent<Button>();
        _roomPrefab = prefab;
        iconImage.sprite = icon;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("Selected: " + _roomPrefab.name);
    }
    
}
