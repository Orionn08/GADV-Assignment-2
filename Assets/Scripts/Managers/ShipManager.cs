using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    public SavingPlayerShip playerShip = new SavingPlayerShip();

    private void Awake()
    {
        Instance = this;
    }
}
//made with the help of Chat GPT
