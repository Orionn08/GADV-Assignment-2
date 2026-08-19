//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script is for any room that can generate a shield for the ship
//it controls how often and when a shield can be gained

using UnityEngine;
using UnityEngine.SceneManagement;

public class Shield : MonoBehaviour
{   
    private Ship _ship; //the ship that the room is under, either Player or Enemy Ship
    public float Cooldown; //sets how often shield is gained, can be changed due to the support room, Engine.
    public float BaseCooldown { get; private set; } //only allows other script to read the variable, not change it.
    //as the name implies, this variable is used as the base cooldown for the Cooldown variable to be reset to in the ResetStats() method, in Ship.cs.
    private float _shieldTimer; //determines when shield can be gained
    private Scene currentScene;

    void Awake()
    {
        BaseCooldown = Cooldown; //will be referred to since the support room Engine can decrease the value of the variable, Cooldown.
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        _ship = GetComponentInParent<Ship>();

        if (currentScene.name != "Combat") return;
        if (_ship == null)
        {
            Debug.LogError($"{name}: No Ship component found in parent objects.");
            return;
        }
        if (Cooldown <= 0)
        {
            Debug.LogError($"{name}: No cooldown has been set or is negative");
            return;
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.

        _shieldTimer = Cooldown + Random.Range(0f, 1f); 
        //sets shield generator to produce a shield after x amount of seconds, according to Cooldown, when the game object is first instantiated.
        //also ensures that shield isn't immediately gained upon being instantiated.
        //a random float, between 0 to 1, is added too to ensure not every single sheild generate will generate a shield at the exact same moment every time.
    } 
    void Update()
    {        
        if(currentScene.name != "Combat" || gameObject.GetComponent<Room>().IsDestroyed == true || !CombatManager.Instance.CombatActive || Cooldown <= 0) return;
        //checks for if this script isn't supposed to do anything; if combat isn't active or if the game isn't even in the Combat scene.

        _shieldTimer -= Time.deltaTime; //Time.deltaTime ensures _shieldTimer goes down steadily each frame.
        if(_shieldTimer <= 0)
        {
            _ship.ShieldGain(1); //calls ShieldGain() in the ship that the room is in to allow the ship to gain 1 sheild.
            _shieldTimer = Cooldown; //_shieldTimer is reset, this time without the additional float since it's no needed now.
        }
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example