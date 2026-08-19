//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    private Ship _ship; //the ship that the room is under, either Player or Enemy Ship
    public int Damage; // Amount of damage the weapon deals, can be changed due to the support room, Reactor.
    public int BaseDamage { get; private set; } //only allows other script to read the variable, not change it.
    //as the name implies, this variable is used as the base damage for the Damage variable to be reset to in the ResetStats() method, in Ship.cs.
    public float Cooldown; // Time between attacks, can be changed due to the support room, Engine.
    public float BaseCooldown { get; private set; } //only allows other script to read the variable, not change it.
    //as the name implies, this variable is used as the base cooldown for the Cooldown variable to be reset to in the ResetStats() method, in Ship.cs.
    private float _attackTimer; //determines when the weapon fires.
    private Scene _currentScene;
    public Room TargetRoom; //stores the target room, determined by CombatManager if the player set this weapon room to target a specific opposing room, 
    //or randomly selected each time the weapon fires, if no specific opposing ship's room was set
    [SerializeField] private GameObject _projectile; //stores the projectile based on the type of weapon room
    //bullet for Machine Gun, laser for Laser Gun and missile for Missile Launcher.

    void Awake()
    {
        BaseCooldown = Cooldown; //will be referred to since the support room Engine can decrease the value of the variable, Cooldown.
        BaseDamage = Damage; //will be referred to since the support room Reactor can increase the value of the variable, Damage.
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
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.

        _attackTimer = Cooldown + 0.25f + Random.Range(0f, 1f);
        //sets the weapon to fire after x amount of seconds, according to Cooldown, when the game object is first instantiated.
        //also ensures that the weapon isn't immediately fired upon being instantiated.
        //a random float, between 0 to 1, is added too to ensure not every single weapon room of the same type 
        // will fire at the exact same moment every time.
    }

    void Update()
    {   
        if (_currentScene.name != "Combat" || gameObject.GetComponent<Room>().IsDestroyed == true || !CombatManager.Instance.CombatActive || 
        Cooldown <= 0 || _ship == null || _ship.OpposingShip == null) return;
        //checks for if this script isn't supposed to do anything; if combat isn't active or if the game isn't even in the Combat scene.
        //also checks if for some reason _ship and its variable, in Ship.cs, OpposingShip is null.

        _attackTimer -= Time.deltaTime; //Time.deltaTime ensures _attackTimer goes down steadily each frame.

        if (_attackTimer <= 0)
        {
            _attackTimer = Cooldown; //_attackTimer is reset, this time without the additional float since it's no needed now.
            if (TargetRoom == null) //if there is currently no opposing room this weapon is targeting, this loop runs.
            {
                TargetRoom = _ship.OpposingShip.SetRandomRoom(); //calls its ship to give it a random room to target, since Ship.cs is aware of the Opposing ship
                //and each Ship.cs is aware of all the rooms under them
                SetUpProjectile();
                TargetRoom = null; //resets TargetRoom to null since it shoudl targeted randomly, unless the player specifically chooses an opposing roomm.
                return;
            }
            SetUpProjectile();
        }
    }

    void SetUpProjectile()
    {
        if (_projectile == null) return; //should not happen, here for safety purposes. 
        
        GameObject ProjectileTransform;
        Projectile Projectile;
        ProjectileTransform = Instantiate(_projectile, transform.position, transform.rotation, transform);
        //the projectile is instantiated, based off _projectile, at the position of the weapon room.
        Projectile = ProjectileTransform.GetComponent<Projectile>(); 
        if (Projectile == null)
        {
            Debug.LogError($"{name}: Projectile does not have Projectile.cs script.");
            return;
        } //again here for safety reasons as this should also not happen.
        Projectile.TargetRoom = TargetRoom;
        Projectile.Damage = Damage;
        Projectile.OpposingShip = _ship.OpposingShip;
        //required variables for the projectile to deal the correct amount of the damage to the correct room.
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example