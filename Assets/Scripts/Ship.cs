//this script sets and creates the ship's health and shield
//it also has functions that will be called by other scripts, either dealing damage or gaining shield (hasn't been implemented yet)

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Ship : MonoBehaviour
{
    private GameObject _ship;
    
    [SerializeField] private int _maxHealth; //can be changed in inspector
    public float currentHealth;
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private Transform _healthBar;
    //sets varibles for ship's health

    private List<GameObject> shieldPoints = new List<GameObject>();
    [SerializeField] private int _maxShield; //can be changed in inspector
    public float currentShield;
    [SerializeField] private GameObject _shieldPoint;
    [SerializeField] private Transform _shieldBar;
    //sets varibles for ship's shield

    [SerializeField] private GameObject _emptyPoint;
    private Scene currentScene;

    void Awake()
    {
        _ship = gameObject;
        currentScene = SceneManager.GetActiveScene();

        currentHealth = currentShield = 0;


        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = 2.1f * i; //determines the x position of the health point
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar); //creates health point under the _healthBar game object
            HealthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            HealthPoint.transform.localPosition = new Vector2(xPos, 0.5f); 
            //ensures that each health point is next to each other but not overlap
            HealthPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
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
            shieldPoints.Add(ShieldPoint);
        }
    }

    public void DamageTaken(int damage)
    {
        currentHealth = currentHealth - damage;
    }

    public void SheildGain(int shield)
    {
        if (currentShield == _maxShield) return;    
        currentShield = currentShield + shield;
    
        for(int i = 0; i < shieldPoints.Count; i++)
        {
            GameObject shieldPoint = shieldPoints[i];
            if (shieldPoint.name.Contains("Empty"))
            {   
                GameObject newShieldPoint = Instantiate(_shieldPoint, _shieldBar); //creates shield point under the _shieldBar game object
                newShieldPoint.name = $"Sheild Point {i+1}";
                shieldPoints[i] = newShieldPoint;
                newShieldPoint.transform.localPosition = shieldPoint.transform.localPosition;
                newShieldPoint.transform.localScale = new Vector2(3, 3);
                Destroy(shieldPoint);
                break;
            }
        }
    } //edited using Chat GPT 

    public void SheildLost(int shield)
    {
        if (currentShield == 0) return;
        currentShield = currentShield - shield;
    }
    //these are shell functions that will be edited accordingly and implemented later
}