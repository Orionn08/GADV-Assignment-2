//this script sets and creates the ship's health and shield
//it also has functions that will be called by other scripts, either dealing damage or gaining shield (hasn't been implemented yet)

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Ship : MonoBehaviour
{
    private GameObject _ship;
    
    private List<GameObject> healthPoints = new List<GameObject>();
    private List<GameObject> sheildPoints = new List<GameObject>();
    [SerializeField] private int _maxHealth, _maxShield; //can be changed in inspector
    [HideInInspector] public int currentHealth, currentShield;
    [SerializeField] private GameObject _healthPoint, _shieldPoint, _emptyPoint;
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

        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = 2.1f * i; //determines the x position of the health point
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar); //creates health point under the _healthBar game object
            HealthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            HealthPoint.transform.localPosition = new Vector2(xPos, 0.5f); 
            //ensures that each health point is next to each other but not overlap
            HealthPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
            healthPoints.Add(HealthPoint);
        }

        for (float i = 0; i < _maxShield; i++) //creates x amount of shield points according to _maxShield
        {
            GameObject ShieldPoint; 
            float xPos = 2.1f * i; //determines the x position of the shield point
            if (currentScene.name == "Combat")
            {
                ShieldPoint = Instantiate(_emptyPoint, _shieldBar); //creates an empty shield point under the _shieldBar game object
                ShieldPoint.name = $"Sheild Point {i+1} (Empty)"; //gives the shield point a name according to the order it was spawned
            }
            else
            {
                ShieldPoint = Instantiate(_shieldPoint, _shieldBar); //creates shield point under the _shieldBar game object
                ShieldPoint.name = $"Sheild Point {i+1}"; //gives the shield point a name according to the order it was spawned
            }
            
            ShieldPoint.transform.localPosition = new Vector2(xPos, 0.5f);
            //ensures that each shield point is next to each other but not overlap
            ShieldPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
            sheildPoints.Add(ShieldPoint);
        }
    }

    public void DamageTaken(int damage)
    {
        if (currentShield > 0) SheildLost(damage);
        else HealthLost(damage);
    }

    public void SheildGain(int shieldGain)
    {
        if (currentShield == _maxShield) return;  
        for (int i = 0; i <= shieldGain -1; i++)
        {
            currentShield++;
            for(int j = 0; j <= sheildPoints.Count -1; j++)
            {
                GameObject sheildPoint = sheildPoints[j];
                if (sheildPoint.name.Contains("Empty"))
                {
                    GameObject newSheildPoint = Instantiate(_shieldPoint, _shieldBar); //creates shield point under the _shieldBar game object
                    newSheildPoint.name = $"Sheild Point {j+1}";
                    sheildPoints[j] = newSheildPoint;
                    newSheildPoint.transform.localPosition = sheildPoint.transform.localPosition;
                    newSheildPoint.transform.localScale = new Vector2(3, 3);
                    Destroy(sheildPoint);
                    break;
                }
            }
        } 
    } //edited using Chat GPT 

    public void SheildLost(int shieldLost)
    {
        int startingShield = currentShield;
        for(int i = 0; i <= shieldLost -1; i++)
        {
            if (currentShield == 0)
            {
                HealthLost(shieldLost -i);
                return;
            }
            GameObject shieldPoint = sheildPoints[startingShield -i -1];
            GameObject emptyPoint = Instantiate(_emptyPoint, _shieldBar); //creates shield point under the _shieldBar game object
            emptyPoint.name = $"Sheild Point {startingShield - i} (Empty)";
            sheildPoints[startingShield -i -1] = emptyPoint;
            emptyPoint.transform.localPosition = shieldPoint.transform.localPosition;
            emptyPoint.transform.localScale = new Vector2(3, 3);
            Destroy(shieldPoint);
            currentShield--;
        }
    }

    public void HealthLost(int healthLost)
    {
        int startingHealth = currentHealth;
        for(int i = 0; i <= healthLost -1; i++)
        {
            if (currentHealth == 0) return;
            
            GameObject healthPoint = healthPoints[startingHealth -i -1];
            GameObject emptyPoint = Instantiate(_emptyPoint, _healthBar); //creates shield point under the _shieldBar game object
            emptyPoint.name = $"Health Point {startingHealth - i} (Empty)";
            healthPoints[startingHealth -i -1] = emptyPoint;
            emptyPoint.transform.localPosition = healthPoint.transform.localPosition;
            emptyPoint.transform.localScale = new Vector2(3, 3);
            Destroy(healthPoint);
            currentHealth--;
        }
    }
    //edited with the help of Chat GPT
}