//this script sets and creates the ship's health and shield
//it also has functions that will be called by other scripts, either dealing damage or gaining shield (hasn't been implemented yet)

using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private int _maxHealth; //can be changed in inspector
    public float currentHealth;
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private Transform _healthBar;
    //sets varibles for ship's health

    [SerializeField] private int _maxShield; //can be changed in inspector
    public float currentShield;
    [SerializeField] private GameObject _shieldPoint;
    [SerializeField] private Transform _shieldBar;
    //sets varibles for ship's shield

    void Awake()
    {
        currentHealth = _maxHealth;
        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = -36.4f + 2.1f * i; //determines the x position of the health point
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar); //creates health point under the _healthBar game object
            HealthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            HealthPoint.transform.localPosition = new Vector2(xPos, 12.5f); 
            //ensures that each health point is next to each other but not overlap
            HealthPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
        }

        currentShield = _maxShield; 
        for (float i = 0; i < _maxShield; i++) //creates x amount of shield points according to _maxShield
        {
            float xPos = -36.4f + 2.1f * i; //determines the x position of the shield point
            GameObject ShieldPoint = Instantiate(_shieldPoint, _shieldBar); //creates shield point under the _shieldBar game object
            ShieldPoint.name = $"Sheild Point {i+1}"; //gives the shield point a name according to the order it was spawned
            ShieldPoint.transform.localPosition = new Vector2(xPos, 10);
            //ensures that each shield point is next to each other but not overlap
            ShieldPoint.transform.localScale = new Vector2(3, 3); //makes the UI easier to see and look more important
        }
    }

    public void DamageTaken(float damage)
    {
        currentHealth = currentHealth - damage;
    }

    public void SheildGain(float sheild)
    {
        currentShield = currentShield + sheild;
    }

    public void SheildLost(float sheild)
    {
        currentShield = currentShield - sheild;
    }
    //these are shell functions that will be edited accordingly and implemented later
}
