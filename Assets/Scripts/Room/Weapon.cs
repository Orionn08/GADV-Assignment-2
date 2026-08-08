//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    private Ship _ship;
    public int damage; // Amount of damage the weapon deals
    public int baseDamage;
    public float cooldown; // Time between attacks
    public float baseCooldown;
    private float _attackTimer;
    private Scene _currentScene;
    public Room targetRoom;

    void Awake()
    {
        baseCooldown = cooldown;
        baseDamage = damage;
    }
    void Start()
    {   
        _currentScene = SceneManager.GetActiveScene();
        _ship = GetComponentInParent<Ship>();

        if (_currentScene.name != "Combat") return;
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
        if (cooldown <= 0)
        {
            Debug.LogError($"{name}: No cooldown has been set or is negative");
            return;
        }
        if (damage <= 0)
        {
            Debug.LogError($"{name}: No damage has been set or is negative");
            return;
        }

        _attackTimer = cooldown + 0.25f + Random.Range(0f, 1f);
    }

    void Update()
    {   
        if (_currentScene.name != "Combat") return;
        if (gameObject.GetComponent<Room>().isDestroyed == true) return;
        if (!CombatManager.Instance.CombatActive) return;

        if(cooldown <= 0) return;
        if (_ship == null || _ship.OpposingShip == null) return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            _attackTimer = cooldown;
            _ship.OpposingShip.DamageTaken(damage, targetRoom);
        }
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example