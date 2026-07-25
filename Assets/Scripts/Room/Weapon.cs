//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{   
    private Ship Ship;
    public int damage; //amount of damage that will be dealt, can be changed in inspector
    public float cooldown; //sets how often the weapon can fire
    private float _attackTimer; //determines when the weapon can fire
    private Scene currentScene;

    void Start()
    {
        Ship = GetComponentInParent<Ship>();
        currentScene = SceneManager.GetActiveScene();
        _attackTimer = cooldown + 0.25f;
    }  //sets the weapon to fire after x amount of seconds (according to _cooldown) when the game object is first instantiated
    //also ensures that the weapon doesn't immediately fire upon being instantiated

    void Update()
    {
        if(currentScene.name != "Combat")
        return;

        _attackTimer -= Time.deltaTime;
        if(_attackTimer <= 0)
        {
            _attackTimer = cooldown;
            Ship.DamageTaken(damage);
        }
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example