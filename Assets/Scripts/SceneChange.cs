using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void StartCombat(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
