using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    public PlayerShip playerShip = new PlayerShip();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
