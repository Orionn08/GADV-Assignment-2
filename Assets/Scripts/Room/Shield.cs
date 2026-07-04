using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{   
    public float _cooldown;
    [SerializeField] private float _shieldTimer;

    void Start()
    {
        _shieldTimer = _cooldown;
    }
    void Update()
    {
        _shieldTimer -= Time.deltaTime;
        GainShield();
    }

    public void GainShield()
    {
        if(_shieldTimer > 0)
        return;

        _shieldTimer = _cooldown;
        Debug.Log($"{name} gain 1 shield");
    }
}
