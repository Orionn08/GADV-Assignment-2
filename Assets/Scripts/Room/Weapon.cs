using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{   
    public float _damage;
    public float _cooldown;
    [SerializeField] private float _attackTimer;

    void Start()
    {
        _attackTimer = _cooldown;
    }
    void Update()
    {
        _attackTimer -= Time.deltaTime;
        Attack();
    }

    public void Attack()
    {
        if(_attackTimer > 0)
        return;

        _attackTimer = _cooldown;
        Debug.Log($"{name} fired dealing {_damage} damage");
    }
}
