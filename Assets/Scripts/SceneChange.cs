using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Transform _rooms;

    public void StartCombat(string sceneName)
    {
        ShipManager.Instance.playerShip.SaveShip(_rooms);

        SceneManager.LoadScene(sceneName);
    }
}
//made with the help of Chat GPT and edited