using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    public float currentHealth;
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private Transform _healthBar;

    void Awake()
    {
        currentHealth = _maxHealth;
        _healthBar = transform.Find("Health Bar");
        for (float i = 0; i < _maxHealth; i++)
        {
            float xPos = -3.1f + 0.5f * i;
            GameObject HealthPoint = Instantiate(_healthPoint, _healthBar);
            HealthPoint.name = $"Health Point {i+1}";
            HealthPoint.transform.localPosition = new Vector2(xPos, 1.7f);
        }
    }

    public void DamageTaken(float damage)
    {
        currentHealth = currentHealth - damage;
    }
}
