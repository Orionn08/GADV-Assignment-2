using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;
    public SavingPlayerShip playerShip = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
//made with the help of Chat GPT
