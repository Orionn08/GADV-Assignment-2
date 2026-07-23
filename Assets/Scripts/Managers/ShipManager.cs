using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    public PlayerShip playerShip = new PlayerShip();

    private void Awake()
    {
        Instance = this;
    }
}
//made with the help of Chat GPT
