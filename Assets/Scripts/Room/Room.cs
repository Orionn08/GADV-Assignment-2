//this script sets and creates the health bar of each room
//it also contains the function for taking damage

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{   
    [HideInInspector] public GameObject prefab;
    private SpriteRenderer _roomRenderer;
    public bool isDestroyed = false; 
    public Ship ship;
    private List<GameObject> _healthPoints = new();
    [SerializeField] private int _maxHealth; //can be changed in inspector
    private int _baseMaxHealth;
    public int currentHealth; //the current health of the room; when this hits 0, the room is considered destroyed
    [SerializeField] private GameObject _point;
    [SerializeField] private Transform _healthBar;
    //sets varibles for room's health
    private Weapon weapon;
    private Shield shield;

    void Awake()
    {
        ship = GetComponentInParent<Ship>();
        weapon = GetComponent<Weapon>();
        shield = GetComponent<Shield>();

        if (_maxHealth <= 0)
        {
            Debug.LogError($"{name}: Max health has not been set or is negative.");
            return;
        }
        if (_point == null)
        {
            Debug.LogError($"{name}: Point has not been set.");
            return;
        }
        if (ship == null)
        {
            Debug.LogError($"{name}: Ship is null");
            return;
        }

        _baseMaxHealth = _maxHealth;
        
        _roomRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = _maxHealth;

        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = -3 + 0.5f * i; //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.RoomHealth);
            healthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
            _healthPoints.Add(healthPoint);
        }

        RefreshSupportEffects();
    }

    public void DamageTaken(int healthLost)
    {
        if (isDestroyed == true) 
        {
            ship.HealthLost(healthLost);
            return;
        }

        int startingHealth = currentHealth;
        for(int i = 0; i <= healthLost -1; i++)
        {
            currentHealth--;
            GameObject healthPoint = _healthPoints[startingHealth -i -1];
            healthPoint.name = $"Health Point {startingHealth -i} (Empty)";
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            if (currentHealth == 0)
            {
                _roomRenderer.color = new Color32(58, 58, 58, 255);
                name = name + " (Destroyed)";
                Destroy(_healthBar.gameObject);
                isDestroyed = true;
                ship.HealthLost(healthLost -i -1);
                return;
            }
        }
    }

    public void RefreshSupportEffects()
    {
        ResetStats();
        foreach(Room room in ship.GetAdjacentRooms(this)) ApplySupport(room);
    }

    private void ResetStats()
    {   
        _maxHealth = _baseMaxHealth;

        if (weapon != null)
        {
            weapon.damage = weapon.baseDamage;
            weapon.cooldown = weapon.baseCooldown;
        }
        else if(shield != null) shield.cooldown = shield.baseCooldown;
    }

    private void ApplySupport(Room supportRoom)
    {
        if (supportRoom.prefab == null)
        {
            Debug.LogError($"{supportRoom.name} has a null prefab!");
        }

        Weapon weapon = GetComponent<Weapon>();
        Shield shield = GetComponent<Shield>();
        if (supportRoom.prefab.name == "Bridge") 
        {
            _maxHealth += 2;
        }
        else if (supportRoom.prefab.name == "Engine")
        {
            if (weapon != null) weapon.cooldown -= 0.5f;
            else if (shield != null) shield.cooldown -= 0.5f;
        }
        else if (supportRoom.prefab.name == "Reactor") 
        {
            if (weapon != null) weapon.damage += 1;
        }
    }

    private void CreateExtraHealthPoints(float extraHealthPoints)
    {
        GameObject lastHealthPoint = _healthPoints[_healthPoints.Count -1];
        float xPosition = lastHealthPoint.transform.position.x;
        for (float i = 0; i < extraHealthPoints; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = xPosition -3 + 0.5f * i -0.5f; //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.RoomHealth);
            healthPoint.name = $"Health Point {_healthPoints.Count +1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
            _healthPoints.Add(healthPoint);
        }
    }
}
