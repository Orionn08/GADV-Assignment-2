//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script sets and creates the ship's health and shield
//it also has functions that will be called by other scripts, either dealing damage or gaining shield
//it also houses the OpposingShip variable, which is used by all weapon rooms to tell them what ship to target.

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Ship : MonoBehaviour
{
    private List<GameObject> _healthPoints = new(); //creates the list for all health points which will be iterated through when losing health.
    private List<GameObject> _sheildPoints = new(); //creates the list for all health points which will be iterated through when gaining or losing shield.
    public List<Room> Rooms { get; private set; } = new(); //used to store all the rooms under the _roomsParent so that the OpposingShip can target them.
    [SerializeField] private Transform _roomsParent; //used to get all the rooms under the ship
    [SerializeField] private int _maxHealth, _maxShield; //can be changed in inspector
    [HideInInspector] public int CurrentHealth; 
    //the current health of the ship; when this hits 0, the room is destroyed, IsDestoryed becomes true and all functions seize.
    [HideInInspector] public int CurrentShield;
    //the current shield of the ship, when this hits 0, the targeted room will start taking damage.
    [SerializeField] private GameObject _point; //used to instantiate health and shield points
    [SerializeField] private Transform _shieldBar, _healthBar; //parent object of sheild points and health points respectively.
    //sets varibles for ship's health and shield
    private Scene currentScene;
    public Ship OpposingShip; //used by weapon rooms to determine what ship to target. 

    public void SetOpposingShip(Ship ship)
    {
        OpposingShip = ship;
    }

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Combat")
        {
            _healthBar.transform.localPosition = new Vector2(-15, 17);
            _shieldBar.transform.localPosition = new Vector2(-15, 13);
        }

        CurrentHealth = _maxHealth; //CurrentHealth set to max, unlike Room.cs since the ship's max health can't be altered. 
        CurrentShield = 0; //sheild should not be set to any positive value since it needs to be gained.

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
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.

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
            float XPos = 1.5f * i; //determines the x position of the shield point
            if (currentScene.name == "Combat") 
            //creates shield points as point type empty if the current scene is Combat since as said previously, the shield needs to be gained
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

            shieldPoint.transform.localPosition = new Vector2(XPos, 0.5f);
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
            if(room != null) Rooms.Add(room);
        } //takes each room in the _roomsParent and stores it in the Rooms list. 
    }

    public List<Room> GetAdjacentRooms(Room centerRoom)
    {
        List<Room> adjacent = new();

        Vector2[] offsets =
        {
            new(-7,0), new(7,0), new(0,4), new(0,-4) 
        }; //determined the positions of the neighbouring rooms, 
        //these values never change from Ship Design to Combat or even the Enemy Forms from all combats.

        foreach (Vector2 offset in offsets)
        {
            Vector2 position = (Vector2)centerRoom.transform.localPosition + offset;
            foreach(Room room in Rooms)
            {
                if((Vector2)room.transform.localPosition == position)
                {
                    adjacent.Add(room);
                    break;
                }
            }
        }
        return adjacent; //returns up to 4 adjecent rooms, depending on the position of centerRoom.
    }

    public Room SetRandomRoom() //gives a random room to the weapon room of the OpposingShip.
    {
        Room randomRoom = Rooms[Random.Range(0, Rooms.Count)];
        return randomRoom;
    }
    public void DamageTaken(int damage, Room targetRoom = null) //calls every time a projectile hits any room in the ship.
    {
        if (targetRoom == null)
        {
            Debug.LogError($"{name}: Target room is null.");
            return;
        } //should never happen, here for precauation.

        if (CurrentShield > 0) 
        {
            ShieldLost(damage, targetRoom);
            return;
        }
        else if(targetRoom.CurrentHealth > 0)
        {
            targetRoom.DamageTaken(damage);
            return;
        }
        else HealthLost(damage);
        //damage in this game is taken in 3 steps, ship's shield first, then targeted room's health and lastly ship's health
    }

    public void ShieldGain(int shieldGain)
    {
        if (CurrentShield == _maxShield) return; //not allowing either ship to gain more shield than their set _maxShield.  
        for (int i = 0; i <= shieldGain -1; i++)
        {
            CurrentShield++;
            for(int j = 0; j <= _sheildPoints.Count -1; j++)
            {
                GameObject ShieldPoint = _sheildPoints[j];
                if (ShieldPoint.name.Contains("Empty"))
                {
                    ShieldPoint.GetComponentInChildren<Point>().SetPoint(PointType.Shield);
                    ShieldPoint.name = $"Sheild Point {CurrentShield}";
                    break;
                }
            }
        }
    } //simplys changes the first sheild point it finds thats of point type empty, via name, and changes it to point type shield.
    //edited using Chat GPT 

    public void ShieldLost(int shieldLost, Room targetRoom)
    {
        int startingShield = CurrentShield;
        for(int i = 0; i <= shieldLost -1; i++)
        {
            CurrentShield--;
            GameObject shieldPoint = _sheildPoints[startingShield -i -1];
            shieldPoint.name = $"Sheild Point {startingShield -i} (Empty)";
            shieldPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            //CurrentSheild goes down, the sheild point gets "(Empty)" added to its name and it becomes point type empty 
            if (CurrentShield == 0)
            {
                if (targetRoom == null)
                {
                    Room randomRoom = Rooms[Random.Range(0, Rooms.Count)];
                    randomRoom.DamageTaken(shieldLost -i -1);
                }
                else targetRoom.DamageTaken(shieldLost -i -1);
                return;
            } //as mentioned once shield is 0, the targeted room starts taking damage.
        }
    }

    public void HealthLost(int healthLost)
    {
        int startingHealth = CurrentHealth;
        for(int i = 0; i <= healthLost -1; i++)
        {
            CurrentHealth--;
            
            GameObject HealthPoint = _healthPoints[startingHealth -i -1];
            HealthPoint.name = $"Health Point {startingHealth -i} (Empty)";
            HealthPoint.GetComponentInChildren<Point>().SetPoint(PointType.Empty);
            if (CurrentHealth == 0) return; //no more damage can be taken and EndCombat will be called by CombatManager in the next frame.
        } 
    } //very similar to ShieldLost as seen above,  
    //CurrentHealth goes down, the health point gets "(Empty)" added to its name and it becomes point type empty 

    public void AddRoom(Room room)
    {
        Rooms.Add(room); //adds room to Rooms list
        room.RefreshSupportEffects();

        foreach (Room neighbour in GetAdjacentRooms(room)) neighbour.RefreshSupportEffects();
        //allows for any room that could've been affected to have its support effects refreshed.
    }

    public void RemoveRoom(Room room)
    {
        List<Room> neighbours = GetAdjacentRooms(room);
        Rooms.Remove(room); //removes room to Rooms list

        foreach (Room neighbour in neighbours) neighbour.RefreshSupportEffects();
        //allows for any room that could've been affected to have its support effects refreshed.
    }

    public void RefreshSupport(Room changedRoom) //called by Room.cs since its not aware of the other rooms in its ship.
    {
        changedRoom.RefreshSupportEffects();

        foreach (Room room in GetAdjacentRooms(changedRoom))
        {
            room.RefreshSupportEffects();
        }
    }
    //edited with the help of Chat GPT
}
//code taken from chat gpt and modified