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
        {
            int Rooms = 0; 
            foreach (Transform transform in _rooms)
            {
                Room room = transform.gameObject.GetComponent<Room>();
                if (room != null) Rooms += 1;
            }
            if (Rooms == 0) 
            {
                PlacementManager.Instance.UponNoRooms();
                return;
            }
            if (GameManager.Instance.CombatNumber <= 3) GameManager.Instance.playerShip.SaveShip(_rooms);
        }
        
        if (GameManager.Instance.CombatNumber > 3)
        {
            SceneManager.LoadScene("End");
            GameManager.Instance.playerShip = new();
            GameManager.Instance.CombatNumber = 1;
            return;
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
