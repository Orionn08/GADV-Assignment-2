//this script sets and creates the ship's health and shield
//it also has functions that will be called by other scripts, either dealing damage or gaining shield (hasn't been implemented yet)

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Ship : MonoBehaviour
{
    private GameObject _ship;
    private List<GameObject> _healthPoints = new();
    private List<GameObject> _sheildPoints = new();
    private List<Room> _rooms = new();
    [SerializeField] private Transform _roomsParent;
    [SerializeField] private int _maxHealth, _maxShield; //can be changed in inspector
    [HideInInspector] public int currentHealth, currentShield;
    [SerializeField] private GameObject _point;
    [SerializeField] private Transform _shieldBar, _healthBar;
    //sets varibles for ship's health and shield
    private Scene currentScene;

    [SerializeField] private Ship _opposingShip;

    public Ship OpposingShip => _opposingShip;

    public void SetOpposingShip(Ship ship)
    {
        _opposingShip = ship;
    }

    void Awake()
    {
        _ship = gameObject;
        currentScene = SceneManager.GetActiveScene();

        currentHealth = _maxHealth; 
        currentShield = 0;

        if (_maxHealth <= 0)
        {
            Debug.LogError($"{name}: Max health has not been set or is negative.");
            return;
        }
        if (_maxShield <= 0)
        {
            Debug.LogError($"{name}: Max shield has not been set or is negative.");
            return;
        }
        if (_roomsParent == null)
        {
            Debug.LogError($"{name}: Rooms parent has not been set.");
            return;
        }

        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = 1.5f * i; //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.ShipHealth);
            healthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 0.5f); 
            //ensures that each health point is next to each other but not overlap
            healthPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
            _healthPoints.Add(healthPoint);
        }

        for (float i = 0; i < _maxShield; i++) //creates x amount of shield points according to _maxShield
        {
            GameObject shieldPoint; 
            float xPos = 1.5f * i; //determines the x position of the shield point
            if (currentScene.name == "Combat")
            {
                shieldPoint = Instantiate(_point, _shieldBar); //creates an empty shield point under the _shieldBar game object
                shieldPoint.name = $"Sheild Point {i+1} (Empty)"; //gives the shield point a name according to the order it was spawned
            }
            else
            {
                shieldPoint = Instantiate(_point, _shieldBar); //creates shield point under the _shieldBar game object
                shieldPoint.GetComponentInChildren<Point>().SetPoint(PointType.Shield);
                shieldPoint.name = $"Sheild Point {i+1}"; //gives the shield point a name according to the order it was spawned
            }

            shieldPoint.transform.localPosition = new Vector2(xPos, 0.5f);
            //ensures that each shield point is next to each other but not overlap
            shieldPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
            _sheildPoints.Add(shieldPoint);
        }
    }
    private void Start()
    {
        foreach (Transform roomTransform in _roomsParent)
        {
            Room room = roomTransform.GetComponent<Room>();
            if(room != null) _rooms.Add(room);
        }
    }

    public List<Room> GetAdjacentRooms(Room centerRoom)
    {
        List<Room> adjacent = new();

        Vector2[] offsets =
        {
            new(-7,0),
            new(7,0),
            new(0,4),
            new(0,-4)
        };

        foreach (Vector2 offset in offsets)
        {
            Vector2 position = (Vector2)centerRoom.transform.localPosition + offset;

            foreach(Room room in _rooms)
            {
                if((Vector2)room.transform.localPosition == position)
                {
                    adjacent.Add(room);
                    break;
                }
            }
        }
        return adjacent;
    }

    public void DamageTaken(int damage, Room targetRoom = null)
    {
        if (currentShield > 0) 
        {
            ShieldLost(damage, targetRoom);
            return;
        }
        else if (targetRoom == null)
        {
            Room RandomRoom = _rooms[Random.Range(0, _rooms.Count)];
            RandomRoom.DamageTaken(damage);
            return;
        }
        else if(targetRoom.currentHealth > 0)
        {
            targetRoom.DamageTaken(damage);
            return;
        }
        else HealthLost(damage);
    }

    public void ShieldGain(int shieldGain)
    {
        if (currentShield == _maxShield) return;  
        for (int i = 0; i <= shieldGain -1; i++)
        {
            currentShield++;
            for(int j = 0; j <= _sheildPoints.Count -1; j++)
            {
                GameObject shieldPoint = _sheildPoints[j];
                if (shieldPoint.name.Contains("Empty"))
                {
                    shieldPoint.GetComponentInChildren<Point>().SetPoint(PointType.Shield);
                    shieldPoint.name = $"Sheild Point {currentShield}";
                    break;
                }
            }
        } 
    } //edited using Chat GPT 

    public void ShieldLost(int shieldLost, Room targetRoom)
    {
        int StartingShield = currentShield;
        for(int i = 0; i <= shieldLost -1; i++)
        {
            currentShield--;
            GameObject ShieldPoint = _sheildPoints[StartingShield -i -1];
            ShieldPoint.name = $"Sheild Point {StartingShield -i} (Empty)";
            ShieldPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            if (currentShield == 0)
            {
                if (targetRoom == null)
                {
                    Room RandomRoom = _rooms[Random.Range(0, _rooms.Count)];
                    RandomRoom.DamageTaken(shieldLost -i -1);
                }
                else targetRoom.DamageTaken(shieldLost -i -1);
                return;
            }
        }
    }

    public void HealthLost(int healthLost)
    {
        int StartingHealth = currentHealth;
        for(int i = 0; i <= healthLost -1; i++)
        {
            currentHealth--;
            
            GameObject HealthPoint = _healthPoints[StartingHealth -i -1];
            HealthPoint.name = $"Health Point {StartingHealth -i} (Empty)";
            HealthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            if (currentHealth == 0) return;
        }
    }

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
        room.CreateHealthPoints();
        room.RefreshSupportEffects();

        foreach (Room neighbour in GetAdjacentRooms(room)) neighbour.RefreshSupportEffects();
    }

    public void RemoveRoom(Room room)
    {
        List<Room> neighbours = GetAdjacentRooms(room);
        _rooms.Remove(room);

        foreach (Room neighbour in neighbours) neighbour.RefreshSupportEffects();
    }

    public void RefreshSupport(Room changedRoom)
    {
        changedRoom.RefreshSupportEffects();

        foreach (Room room in GetAdjacentRooms(changedRoom))
        {
            room.RefreshSupportEffects();
        }
    }
    //edited with the help of Chat GPT
}