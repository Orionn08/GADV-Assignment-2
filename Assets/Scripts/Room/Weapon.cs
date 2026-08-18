//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    private Ship _ship;
    public int Damage; // Amount of damage the weapon deals
    public int BaseDamage { get; private set; }
    public float Cooldown; // Time between attacks
    public float BaseCooldown { get; private set; }
    private float _attackTimer;
    private Scene _currentScene;
    public Room TargetRoom;
    [SerializeField] private GameObject _projectile;

    void Awake()
    {
        BaseCooldown = Cooldown;
        BaseDamage = Damage;
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
        if (Cooldown <= 0)
        {
            Debug.LogError($"{name}: No cooldown has been set or is negative");
            return;
        }
        if (Damage <= 0)
        {
            Debug.LogError($"{name}: No damage has been set or is negative");
            return;
        }
        if (_projectile == null)
        {
            Debug.LogError($"{name}: No projectile has been set");
            return;
        }

        _attackTimer = Cooldown + 0.25f + Random.Range(0f, 1f);
    }

    void Update()
    {   
        if (_currentScene.name != "Combat") return;
        if (gameObject.GetComponent<Room>().IsDestroyed == true) return;
        if (!CombatManager.Instance.CombatActive) return;

        if(Cooldown <= 0) return;
        if (_ship == null || _ship.OpposingShip == null) return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            _attackTimer = Cooldown;
            if (TargetRoom == null)
            {
                TargetRoom = _ship.OpposingShip.SetRandomRoom();
                SetUpProjectile();
                TargetRoom = null;
                return;
            }
            SetUpProjectile();
        }
    }

    void SetUpProjectile()
    {
        GameObject ProjectileTransform;
        Projectile Projectile;
        ProjectileTransform = Instantiate(_projectile, transform.position, transform.rotation, transform);
        Projectile = ProjectileTransform.GetComponent<Projectile>();
        if (Projectile == null)
        {
            Debug.LogError($"{name}: Projectile does not have Projectile.cs script.");
            return;
        }
        Projectile.TargetRoom = TargetRoom;
        Projectile.Damage = Damage;
        Projectile.OpposingShip = _ship.OpposingShip;
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example