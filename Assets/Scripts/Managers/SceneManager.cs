//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script manages moving from scene to scene, including ensuring that the player ship's layout is saved.

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
        //only moving between these 2 scenes require the saving of the player ship's layout so the current scene is identified and then a check is done.
        {
            int rooms = 0; //resets the rooms variable
            foreach (Transform transform in _rooms)
            {
                Room room = transform.gameObject.GetComponent<Room>();
                if (room != null) rooms += 1;
            } //checks the amount of rooms in the _rooms transform; slots do not count.
            if (rooms == 0)
            {
                PlacementManager.Instance.UponNoRooms();
                return;
            } //combat can't start with 0 rooms in thr _rooms transform so this loop calls a method from PlacementManager 
            //and prevents the movement from the Ship Design scene to the Combat scene
            if (GameManager.Instance.CombatNumber <= 3) GameManager.Instance.PlayerShip.SaveShip(_rooms);
            //checks if the combat was before the last combat, since the player ship's layout won't need to saved upon defeating the enemy in the final combat.
        }
        
        if (GameManager.Instance.CombatNumber > 3) //only runs if the 3rd combat was just completed.
        {
            SceneManager.LoadScene("End"); //brings the player to the End scene where they can choose to restart the game.
            GameManager.Instance.PlayerShip = new(); //resets the player ship's layout to ensure it doesn't carry over if the game is restarted.
            GameManager.Instance.CombatNumber = 1; //resets the combat back to 1 to allow the game to properly loop.
            return;
        }
        
        SceneManager.LoadScene(sceneName); //loads the respective scene given by the button when its clicked, OnClick().
    }
}
