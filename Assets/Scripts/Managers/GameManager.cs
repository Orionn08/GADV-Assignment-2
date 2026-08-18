using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SavingPlayerShip playerShip = new();
    [HideInInspector] public int CombatNumber = 1;
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
