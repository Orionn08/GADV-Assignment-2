using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Transform _rooms;
    void Awake()
    {
        if (_rooms == null)
        {
            Debug.LogError($"{name}: Rooms parent has not been set or is negative.");
            return;
        }
    }
    public void StartCombat(string sceneName)
    {
        ShipManager.Instance.playerShip.SaveShip(_rooms);

        SceneManager.LoadScene(sceneName);
    }
}
//made with the help of Chat GPT and edited