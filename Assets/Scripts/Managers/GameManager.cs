//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//the purpose of this script right now is to store the combat number and storing the player ship's layout between the Ship Design and Combat scenes.
//additional purposes can be added if it is important for the values to be constant or if something needs to be saved between scenes.

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //only allows other script to read the variable, not change it.
    //since only 1 Game Manager is meant to exist at a time, this ensures every script can easily reference it.
    public SavingPlayerShip PlayerShip = new(); //as mentioned above, this will store the ship layout of the player ship.
    [HideInInspector] public int CombatNumber = 1; //stores the current combat number to be used in other scripts, mainly CombatManager.cs.
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } // Checks if another instance already exists and destroys this duplicate
        Instance = this;
        DontDestroyOnLoad(gameObject); //ensures the object will not be deleted and can hence transfer the information it stores from scene to scene
    }
}

//made with the help of Chat GPT
