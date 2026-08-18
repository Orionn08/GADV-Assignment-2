using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Room targetRoom;
    public int damage;
    [SerializeField] private float _projectileSpeed;
    public Ship opposingShip;


    void Start()
    {
        if (targetRoom == null)
        {
            Debug.LogError($"{name}: Target room has not been set.");
            return;
        }
        if (damage <= 0)
        {
            Debug.LogError($"{name}: Damage has not been set or is negative.");
            return;
        }
        if (_projectileSpeed <= 0)
        {
            Debug.LogError($"{name}: Projectile speed has not been set or is negative.");
            return;
        }
        if (opposingShip == null)
        {
            Debug.LogError($"{name}: Opposing ship has not been set.");
            return;
        }
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetRoom.transform.position, _projectileSpeed * Time.deltaTime);
        if (transform.position == targetRoom.transform.position)
        {
            opposingShip.DamageTaken(damage, targetRoom);
            Destroy(gameObject);
        }
        if (CombatManager.Instance.CombatActive == false) Destroy(gameObject);
    } 
}
