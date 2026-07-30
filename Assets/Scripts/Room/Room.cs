//this script sets and creates the health bar of each room
//it also contains the function for taking damage

using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{   
    [HideInInspector] public GameObject prefab;
    private SpriteRenderer _roomRenderer;
    public bool isDestroyed = false; 
    public Ship ship;
    private List<GameObject> healthPoints = new();
    [SerializeField] private int _maxHealth; //can be changed in inspector
    public int currentHealth; //the current health of the room; when this hits 0, the room is considered destroyed
    [SerializeField] private GameObject _point;
    [SerializeField] private Transform _healthBar;
    //sets varibles for room's health

    void Awake()
    {
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
        currentHealth = _maxHealth;
        //finds the Health Bar object so that it can be the parent of the health points objects
        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = -3 + 0.7f * i; //determines the x position of the health point
            GameObject healthPoint = Instantiate(_point, _healthBar); //creates health point under the _healthBar game object
            healthPoint.GetComponentInChildren<Point>().SetPoint(PointType.RoomHealth);
            healthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            healthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
            healthPoints.Add(healthPoint);
        }
        _roomRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        ship = GetComponentInParent<Ship>();
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
            GameObject healthPoint = healthPoints[startingHealth -i -1];
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
    //a shell functions that will be edited accordingly and implemented later
}
