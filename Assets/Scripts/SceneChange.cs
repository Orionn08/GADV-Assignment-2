using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Transform _rooms;
    public static SceneChange Instance;
    void Awake()
    {

        if (_rooms == null)
        {
            Debug.LogError($"{name}: Rooms parent has not been set or is negative.");
            return;
        }
    }
    public void ChangeScene(string sceneName)
    {
        ShipManager.Instance.playerShip.SaveShip(_rooms);
        SceneManager.LoadScene(sceneName);
    }
}
//made with the help of Chat GPT and edited