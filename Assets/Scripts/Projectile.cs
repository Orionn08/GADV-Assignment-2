//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//given to every projectile fired from a weapon, both the player and enemy ships.
//this script takes the damage and target of the weapon room it got spawned by and deals that damage to whichever room was the target of the weapon.

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Room TargetRoom;
    public int Damage;
    public Ship OpposingShip;
    //these 3 variables are set by the Weapon.cs script in SetUpProjectile().
    [SerializeField] private float _projectileSpeed; //each projectile has a different speed which determines how fast they can hit their target.
    
    void Start()
    {
        if (TargetRoom == null)
        {
            Debug.LogError($"{name}: Target room has not been set.");
            return;
        }
        if (Damage <= 0)
        {
            Debug.LogError($"{name}: Damage has not been set or is negative.");
            return;
        }
        if (_projectileSpeed <= 0)
        {
            Debug.LogError($"{name}: Projectile speed has not been set or is negative.");
            return;
        }
        if (OpposingShip == null)
        {
            Debug.LogError($"{name}: Opposing ship has not been set.");
            return;
        } //bunch of safety precautions as the script needs everything here to be set properly in order to work.
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetRoom.transform.position, _projectileSpeed * Time.deltaTime);
        //moves the projectile, by changing its position, slowly towards the position of the room its targeting, speed being based on _projectileSpeed;
        //Time.deltaTime is used to ensure that the movement every frame is roughly the same and that its smooth.

        if (transform.position == TargetRoom.transform.position)
        {
            OpposingShip.DamageTaken(Damage, TargetRoom);
            Destroy(gameObject);
        }//once the projectile has reached its targeted room, meaning it got to the position of the targeted room, it deals the damage, 
        //based on the weapon room it got fired from, by calling the DamageTaken() method in the OpposingShip. 

        if (CombatManager.Instance.CombatActive == false) Destroy(gameObject);
        //all projectiles get deleted the moment combat isn't active.
    } 
}
