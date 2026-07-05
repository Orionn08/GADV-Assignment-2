//this script is for all weapons that can damage opponent's ships
//it controls how often and when a weapon can be fired

using UnityEngine;

public class Weapon : MonoBehaviour
{   
    public float damage; //amount of damage that will be dealt, can be changed in inspector
    public float cooldown; //sets how often the weapon can fire
    [SerializeField] private float _attackTimer; //determines when the weapon can fire

    void Start()
    {
        _attackTimer = cooldown; 
    }  //sets the weapon to fire after x amount of seconds (according to _cooldown) when the game object is first instantiated
    //also ensures that the weapon doesn't immediately fire upon being instantiated

    void Update()
    {
        _attackTimer -= Time.deltaTime; //ensures _attackTimer goes down at a constant rate
        Attack();
    }

    public void Attack()
    {
        if(_attackTimer > 0)
        return; //doesn't do anything if _attackTimer isn't below 0

        Debug.Log($"{name} fired dealing {damage} damage");
        //for now the weapon doesn't actually do damage so this log states which weapon room fired and how much damage it's supposed to do
        _attackTimer = cooldown; //sets the weapon to fire after x amount of seconds (according to _cooldown) after the weapon has fired
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example