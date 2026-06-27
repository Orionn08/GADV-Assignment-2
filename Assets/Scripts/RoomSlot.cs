using UnityEngine;

public class RoomSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}
