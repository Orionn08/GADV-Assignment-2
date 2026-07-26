//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    private Ship _ship;

    public int damage; // Amount of damage the weapon deals
    public float cooldown; // Time between attacks
    private float _attackTimer;
    private Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "Combat")
            return;

        _ship = GetComponentInParent<Ship>();

        if (_ship == null)
        {
            Debug.LogError($"{name}: No Ship component found in parent objects.");
            return;
        }

        if (_ship.OpposingShip == null)
        {
            Debug.LogError($"{name}: Opposing ship has not been assigned.");
            return;
        }

        _attackTimer = cooldown + 0.25f;
    }

    private void Update()
    {
        if (currentScene.name != "Combat") return;
        if(cooldown <= 0) return;
        if (_ship == null || _ship.OpposingShip == null) return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            _attackTimer = cooldown;
            _ship.OpposingShip.DamageTaken(damage);
        }
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example