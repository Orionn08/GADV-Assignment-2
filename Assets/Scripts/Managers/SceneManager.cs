using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager_ : MonoBehaviour
{
    private Scene _currentScene;
    [SerializeField] private Transform _rooms;
    public void ChangeScene(string sceneName)
    {
        _currentScene = SceneManager.GetActiveScene();
        if (_currentScene.name == "Combat" || _currentScene.name == "Ship Design") 
        if (GameManager.Instance.CombatNumber <= 3) GameManager.Instance.playerShip.SaveShip(_rooms);
        
        if (GameManager.Instance.CombatNumber > 3)
        {
            SceneManager.LoadScene("End");
            GameManager.Instance.CombatNumber = 1;
            return;
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
