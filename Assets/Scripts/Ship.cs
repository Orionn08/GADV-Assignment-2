using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    public float currentHealth;
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private Transform _healthBar;
    [SerializeField] private int _maxShield;
    public float currentShield;
    [SerializeField] private GameObject _shieldPoint;
    [SerializeField] private Transform _shieldBar;

    void Awake()
    {
        currentHealth = _maxHealth;
        for (float i = 0; i < _maxHealth; i++)
        {
            float xPos = -36.4f + 2.1f * i;
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar);
            HealthPoint.name = $"Health Point {i+1}";
            HealthPoint.transform.localPosition = new Vector2(xPos, 12.5f);
            HealthPoint.transform.localScale = new Vector2(3, 3);
        }

        currentShield = _maxShield;
        for (float i = 0; i < _maxShield; i++)
        {
            float xPos = -36.4f + 2.1f * i;
            GameObject ShieldPoint = Instantiate(_shieldPoint, _shieldBar);
            ShieldPoint.name = $"Sheild Point {i+1}";
            ShieldPoint.transform.localPosition = new Vector2(xPos, 10);
            ShieldPoint.transform.localScale = new Vector2(3, 3);
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
}
