using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Room TargetRoom;
    public int Damage;
    [SerializeField] private float _projectileSpeed;
    public Ship OpposingShip;


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
        }
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetRoom.transform.position, _projectileSpeed * Time.deltaTime);
        if (transform.position == TargetRoom.transform.position)
        {
            OpposingShip.DamageTaken(Damage, TargetRoom);
            Destroy(gameObject);
        }
        if (CombatManager.Instance.CombatActive == false) Destroy(gameObject);
    } 
}
