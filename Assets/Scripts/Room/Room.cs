//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script sets and creates the health bar of each room
//it also contains the function for taking damage

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class Room : MonoBehaviour
{   
    public GameObject Prefab; 
    //needed for a few reasons, saving and rebuilding the player ship in SavingPlayerShip.cs and PlayerShipBuilder.cs, 
    //also used by PlacementManager.cs when chekcing for the amount of rooms of a specific room type
    public float Limit; //an optional variable to limit the amount of rooms the player can place of this room type
    private SpriteRenderer _roomRenderer; //will be set to grey upon the room being destroyed, meaning hitting 0 health.
    public bool IsDestroyed { get; private set; } = false; //only allows other script to read the variable, not change it.
    //used to seize all functions of a room when destroyed, meaning hitting 0 health.
    public Ship Ship; //stores the ship the room is under
    private List<GameObject> _healthPoints = new(); //creates the list for all health points which will be iterated through when losing health
    public int MaxHealth; //can be changed due to the support room Bridge, which can increase room health.
    [SerializeField] private int _baseMaxHealth; 
    //as the name implies, this variable is used as the base max health for the MaxHealth variable to be reset to in the ResetStats() method, in Ship.cs.
    public int CurrentHealth; //the current health of the room; when this hits 0, the room is destroyed, IsDestoryed becomes true and all functions seize.
    [SerializeField] private GameObject _point; //used to instantiate room health points
    [SerializeField] private Transform _healthBar; //parent object of health points
    //sets varibles for room's health
    private Weapon weapon;
    private Shield shield;
    //used to determine if the room is a weapon room, shield room or support room
    [SerializeField] private TMP_Text _cooldownText, _damageText;
    //values of the cooldown and damage of the room, if there is any, to be shown to the player in both the Ship Design and Combat scenes.
    [SerializeField] private GameObject _decreasedCooldown, _increasedDamage;
    //game objects that will be set as active if a support room affect the room.
    [SerializeField] private GameObject _canvas;
    //disabled upon getting destroyed
    private Scene _currentScene;
    public GameObject TargetIcon; 

    void Awake()
    {
        Ship = GetComponentInParent<Ship>();
        weapon = GetComponent<Weapon>();
        shield = GetComponent<Shield>();

        _currentScene = SceneManager.GetActiveScene();

        if (MaxHealth <= 0)
        {
            Debug.LogError($"{name}: Max health has not been set or is negative.");
            return;
        }
        if (_point == null)
        {
            Debug.LogError($"{name}: Point has not been set.");
            return;
        }
        if (Ship == null)
        {
            Debug.LogError($"{name}: Ship is null.");
            return;
        }
        if (weapon != null || shield != null) //only needed if the room has a cooldown, weapon and shield rooms.
        {
            if (_canvas == null)
            {
                Debug.LogError($"{name}: Canvas has not been set.");
                return;
            }
            if (_cooldownText == null)
            {
                Debug.LogError($"{name}: Cooldown text has not been set.");
                return;
            }
        }
        if (weapon != null) //only needed if the room has damage, weapon rooms.
        {
            if (_damageText == null)
            {
                Debug.LogError($"{name}: Damage text has not been set.");
                return;
            }
        }
        if (TargetIcon == null)
        {
            Debug.LogError($"{name}: Target room has not been set.");
            return;
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.

        _baseMaxHealth = MaxHealth; //sets _baseMaxHealth to the value that MaxHealth was given.
        CreateHealthPoints();
        
        _roomRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>(); //needed for room to be turned grey upon getting destoryed
    }

    void Start()
    {
        if (_cooldownText != null)
        {
            if (weapon != null) _cooldownText.text = $"{weapon.Cooldown} s";
            if (shield != null) _cooldownText.text = $"{shield.Cooldown} s";
        }
        if (_damageText != null && weapon != null) _damageText.text = $"{weapon.Damage}";
        //assigns the text so the player can see. the player can then better plan for the upcoming combat
        
        RefreshSupportEffects();
    }

    public void CreateHealthPoints()
    {
        CurrentHealth = MaxHealth;
        for (float i = 0; i < MaxHealth; i++) //creates x amount of health points according to MaxHealth
        {
            float xPos = -3 + 0.5f * i; //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.RoomHealth);
            healthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
            _healthPoints.Add(healthPoint);
        }
    }

    public void Target(bool state) //sets the highlight object to the opposite state it was originally at
    {
        TargetIcon.SetActive(state); //turns on and off highlight so the player can see what room they are hovering over
    }

    public void DamageTaken(int healthLost)
    {
        if (IsDestroyed == true) 
        {
            Ship.HealthLost(healthLost); //only when the targeted room is at 0 health will the ship the room is under take damage. 
            return;
        }

        int startingHealth = CurrentHealth; //ensures the value wont be changed while being used to iterate through. 
        for(int i = 0; i <= healthLost -1; i++)
        {
            CurrentHealth--;
            GameObject healthPoint = _healthPoints[startingHealth -i -1];
            healthPoint.name = $"Health Point {startingHealth -i} (Empty)";
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty); //indicates to the player that this health point has been lost
            if (CurrentHealth == 0)
            {
                _roomRenderer.color = new Color32(58, 58, 58, 255); //color to show the room is destroyed.
                name = name + " (Destroyed)"; 
                Destroy(_healthBar.gameObject);
                IsDestroyed = true;
                if (_canvas != null) _canvas.SetActive(false);
                if (weapon != null) weapon.TargetRoom = null;
                Ship.RefreshSupport(this); //refreshes the support rooms of this room and the 4 surrouding rooms
                Ship.HealthLost(healthLost -i -1); //calls the ship this room is under to lose the remaining health, 
                // based on whatever damage this room didnt take since it hit 0 health
                return;
            }
        }
    }

    public void RefreshSupportEffects()
    {
        if (IsDestroyed == true) return; //has no effect if room is destroyed

        ResetStats();
        foreach(Room room in Ship.GetAdjacentRooms(this)) ApplySupport(room);
    }

    private void ResetStats()
    {   
        if (_currentScene.name != "Combat") CurrentHealth = _baseMaxHealth;
        else if (_baseMaxHealth != MaxHealth) CurrentHealth -= 2; //checks if the MaxHealth variable's value was changed.
        //doesn't allow for rooms in Combat scene to magically gain their health back.
        MaxHealth = _baseMaxHealth;
        if (MaxHealth < _healthPoints.Count) DeleteExcessHealthPoints(_healthPoints.Count - MaxHealth); 
        //removes whatever extra health points were created if MaxHealth was changed

        if (weapon != null)
        {
            _increasedDamage.SetActive(false);
            weapon.Damage = weapon.BaseDamage;
            _damageText.text = $"{weapon.Damage}";
            _decreasedCooldown.SetActive(false);
            weapon.Cooldown = weapon.BaseCooldown;
            _cooldownText.text = $"{weapon.Cooldown} s";
        }
        else if(shield != null)
        {
            _decreasedCooldown.SetActive(false);
            shield.Cooldown = shield.BaseCooldown;
           _cooldownText.text = $"{shield.Cooldown} s";
        }
        //resets all values in Shield and Weapon.cs back to their base values
    }

    private void ApplySupport(Room supportRoom)
    {
        if(supportRoom.IsDestroyed == true) return; //if the support room is destroyed, it has no effect
        if (supportRoom.Prefab == null)
        {
            Debug.LogError($"{supportRoom.name} has a null prefab!");
            return;
        } //should not be true; here as precaution.

        if (supportRoom.Prefab.name == "Bridge") //checks prefab based on name to determine if its a support room.
        {
            MaxHealth += 2; CurrentHealth += 2;  
            if (MaxHealth > _healthPoints.Count) CreateExtraHealthPoints(2);
        }
        else if (supportRoom.Prefab.name == "Engine")
        {
            if (weapon != null)
            {
                _decreasedCooldown.SetActive(true); //set to active to indicate to the player that this room has been boosted.
                weapon.Cooldown = weapon.Cooldown * 0.8f;
                _cooldownText.text = $"{weapon.Cooldown} s"; //value is changed to indicate to the player that this room has been boosted.
            }
            else if (shield != null)
            {
                _decreasedCooldown.SetActive(true); //set to active to indicate to the player that this room has been boosted.
                shield.Cooldown = shield.Cooldown * 0.8f;
                _cooldownText.text = $"{shield.Cooldown} s"; //value is changed to indicate to the player that this room has been boosted.
            }
        }
        else if (supportRoom.Prefab.name == "Reactor")
        {
            if (weapon != null)
            {
                _increasedDamage.SetActive(true); //set to active to indicate to the player that this room has been boosted.
                weapon.Damage += 1;
                _damageText.text = $"{weapon.Damage}"; //value is changed to indicate to the player that this room has been boosted.
            }
        } //applies the different effects of the support rooms
    }    

    private void CreateExtraHealthPoints(float extraHealthPoints)
    {
        GameObject lastHealthPoint = _healthPoints[_healthPoints.Count -1];
        float xPosition = lastHealthPoint.transform.localPosition.x;
        for (float i = 0; i < extraHealthPoints; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = xPosition + (0.5f * (i+1)); //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            if (_currentScene.name == "Combat" && CurrentHealth < _baseMaxHealth) healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            else healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.ExtraRoomHealth);
            healthPoint.name = $"Health Point {_healthPoints.Count +1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
            _healthPoints.Add(healthPoint);
        }
    }

    private void DeleteExcessHealthPoints(float excessHealthPoints)
    {
        for (int i = 0; i < excessHealthPoints; i++)
        {
            GameObject lastHealthPoint = _healthPoints[_healthPoints.Count -1];
            _healthPoints.Remove(lastHealthPoint);
            Destroy(lastHealthPoint);
        } //simply just removes the last health points, which are the extras, of the room.
    }
}