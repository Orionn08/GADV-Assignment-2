//this script sets and creates the health bar of each room
//it also contains the function for taking damage

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Room : MonoBehaviour
{   
    public GameObject prefab;
    public float limit;
    private SpriteRenderer _roomRenderer;
    public bool IsDestroyed { get; private set; } = false;
    public Ship ship;
    private List<GameObject> _healthPoints = new();
    public int MaxHealth; //can be changed in inspector
    [SerializeField] private int _baseMaxHealth;
    public int CurrentHealth; //the current health of the room; when this hits 0, the room is considered destroyed
    [SerializeField] private GameObject _point;
    [SerializeField] private Transform _healthBar;
    //sets varibles for room's health
    private Weapon weapon;
    private Shield shield;
    [SerializeField] private TMP_Text _cooldownText, _damageText;
    [SerializeField] private GameObject _decreasedCooldown, _increasedDamage, _canvas;
    private Scene _currentScene;
    public GameObject target;

    void Awake()
    {
        ship = GetComponentInParent<Ship>();
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
        if (ship == null)
        {
            Debug.LogError($"{name}: Ship is null.");
            return;
        }
        if (weapon != null || shield != null)
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
        if (weapon != null)
        {
            if (_damageText == null)
            {
                Debug.LogError($"{name}: Damage text has not been set.");
                return;
            }
        }
        if (target == null)
        {
            Debug.LogError($"{name}: Target has not been set.");
            return;
        }

        _baseMaxHealth = MaxHealth;
        CreateHealthPoints();
        
        _roomRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (_cooldownText != null)
        {
            if (weapon != null) _cooldownText.text = $"{weapon.Cooldown} s";
            if (shield != null) _cooldownText.text = $"{shield.Cooldown} s";
        }
        if (_damageText != null && weapon != null) _damageText.text = $"{weapon.Damage}";
        
        RefreshSupportEffects();
    }

    public void CreateHealthPoints()
    {
        CurrentHealth = MaxHealth;
        for (float i = 0; i < MaxHealth; i++) //creates x amount of health points according to _maxHealth
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
        target.SetActive(state); //turns on and off highlight so the player can see what room they are hovering over
    }

    public void DamageTaken(int healthLost)
    {
        if (IsDestroyed == true) 
        {
            ship.HealthLost(healthLost);
            return;
        }

        int startingHealth = CurrentHealth;
        for(int i = 0; i <= healthLost -1; i++)
        {
            CurrentHealth--;
            GameObject healthPoint = _healthPoints[startingHealth -i -1];
            healthPoint.name = $"Health Point {startingHealth -i} (Empty)";
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            if (CurrentHealth == 0)
            {
                _roomRenderer.color = new Color32(58, 58, 58, 255);
                name = name + " (Destroyed)";
                Destroy(_healthBar.gameObject);
                IsDestroyed = true;
                if (_canvas != null) _canvas.SetActive(false);
                if (weapon != null) weapon.TargetRoom = null;
                ship.RefreshSupport(this);
                ship.HealthLost(healthLost -i -1);
                return;
            }
        }
    }

    public void RefreshSupportEffects()
    {
        if (IsDestroyed == true) return;

        ResetStats();
        foreach(Room room in ship.GetAdjacentRooms(this)) ApplySupport(room);
    }

    private void ResetStats()
    {   
        if (_currentScene.name != "Combat") CurrentHealth = _baseMaxHealth;
        else if (_baseMaxHealth != MaxHealth) CurrentHealth -= 2;
        MaxHealth = _baseMaxHealth;
        if (MaxHealth < _healthPoints.Count) DeleteExcessHealthPoints(_healthPoints.Count - MaxHealth);

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
    }

    private void ApplySupport(Room supportRoom)
    {
        if(supportRoom.IsDestroyed == true) return;
        if (supportRoom.prefab == null)
        {
            Debug.LogError($"{supportRoom.name} has a null prefab!");
            return;
        }

        if (supportRoom.prefab.name == "Bridge") 
        {
            MaxHealth += 2; CurrentHealth += 2;  
            if (MaxHealth > _healthPoints.Count) CreateExtraHealthPoints(2);
        }
        else if (supportRoom.prefab.name == "Engine")
        {
            if (weapon != null)
            {
                _decreasedCooldown.SetActive(true);
                weapon.Cooldown = weapon.Cooldown * 0.8f;
                _cooldownText.text = $"{weapon.Cooldown} s";
            }
            else if (shield != null)
            {
                _decreasedCooldown.SetActive(true);
                shield.Cooldown = shield.Cooldown * 0.8f;
                _cooldownText.text = $"{shield.Cooldown} s";
            }
        }
        else if (supportRoom.prefab.name == "Reactor")
        {
            if (weapon != null)
            {
                _increasedDamage.SetActive(true);
                weapon.Damage += 1;
                _damageText.text = $"{weapon.Damage}";
            }
        }
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
        }
    }
}