//this script is for any room that can generate a shield for the ship
//it controls how often and when a shield can be gained

using UnityEngine;
using UnityEngine.SceneManagement;

public class Shield : MonoBehaviour
{   
    private Ship Ship;
    public float cooldown; //sets how often shield is gained, can be changed in inspector
    private float _shieldTimer; //determines when shield can be gained
    private Scene currentScene;

    void Start()
    {
        Ship = GetComponentInParent<Ship>();
        currentScene = SceneManager.GetActiveScene();
        _shieldTimer = cooldown;
    } //sets shield generator to produce a shield after x amount of seconds (according to _cooldown) when the game object is first instantiated
    //also ensures that shield isn't immediately gained upon being instantiated
    void Update()
    {   
        if(currentScene.name != "Combat")
        return;

        _shieldTimer -= Time.deltaTime;
        if(_shieldTimer <= 0)
        {
            Ship.SheildGain(1);
            _shieldTimer = cooldown;
        }
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example