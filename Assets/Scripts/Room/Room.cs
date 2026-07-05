//this script sets and creates the health bar of each room
//it also contains the function for taking damage

using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int _maxHealth; //can be changed in inspector
    public float currentHealth; //the current health of the room; when this hits 0, the room is considered destroyed
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private Transform _healthBar;
    //sets varibles for room's health

    void Awake()
    {
        currentHealth = _maxHealth;
        _healthBar = transform.Find("Health Bar"); 
        //finds the Health Bar object so that it can be the parent of the health points objects
        for (float i = 0; i < _maxHealth; i++) //creates x amount of health points according to _maxHealth
        {
            float xPos = -3 + 0.7f * i; //determines the x position of the health point
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar); //creates health point under the _healthBar game object
            HealthPoint.name = $"Health Point {i+1}"; //gives the health point a name according to the order it was spawned
            HealthPoint.transform.localPosition = new Vector2(xPos, 1.5f);
            //ensures that each health point is next to each other but not overlap
        }
    }

    public void DamageTaken(float damage)
    {
        currentHealth = currentHealth - damage;
    }
    //a shell functions that will be edited accordingly and implemented later
}
