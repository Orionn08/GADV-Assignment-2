//this script is for any room that can generate a shield for the ship
//it controls how often and when a shield can be gained

using UnityEngine;

public class Shield : MonoBehaviour
{   
    public float cooldown; //sets how often shield is gained, can be changed in inspector
    [SerializeField] private float _shieldTimer; //determines when shield can be gained

    void Start()
    {
        _shieldTimer = cooldown;
    } //sets shield generator to produce a shield after x amount of seconds (according to _cooldown) when the game object is first instantiated
    //also ensures that shield isn't immediately gained upon being instantiated

    void Update()
    {
        _shieldTimer -= Time.deltaTime; //ensures _shieldTimer goes down at a constant rate
        GainShield();
    }

    public void GainShield()
    {
        if(_shieldTimer > 0)
        return; //doesn't do anything if _shieldTimer isn't below 0

        Debug.Log($"{name} gain 1 shield");
        //for now shield can't actually be gained so this log states which shield generator produced a shield
        _shieldTimer = cooldown; //sets the shield generator to gain another shield after x amount of seconds (according to _cooldown) after a shield was gained
    }
}
//rough code structure from https://www.youtube.com/watch?v=N4SFyoLBOS4, the 3rd example