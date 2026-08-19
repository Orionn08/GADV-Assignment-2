//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script manages the entire combat, determining if combat is active, shows the text in the Combat Scene, 
// shows the outcome screen (depending on what happens in the combat), stores the variable of the current weapon the player wants to target a specific enemy room 
// it also contains all of the enemy forms that can spawn in combat

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Security.Cryptography;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; } //only allows other script to read the variable, not change it.
    //since only 1 Combat Manager is meant to exist at a time, this ensures every script can easily reference it.
    public bool CombatActive { get; private set; } = true; //only allows other script to read the variable, not change it.
    //used to determine if combat is still active
    //once set to false, all weapons and shields stop working, projectiles disappear, no more hovering over rooms/slots or targeting rooms and all text disappear
    //effectively just stops all activity when false
    [SerializeField] private Ship _playerShip, _enemyShip; //needed when assigning the OpposingShip in Ship.cs for both the Player and Enemy ships.
    [SerializeField] private GameObject _victoryScreen, _defeatScreen, _drawScreen;
    //the different outcomes of the combat; only 1 should show based on what happened in the combat; player winning, enemy winning or a draw (timeout)
    public Weapon selectedWeapon { get; private set; } //only allows other script to read the variable, not change it.
    //used in the targeting system; stores the current weapon the player wants to target a specific enemy room.
    [SerializeField] private TMP_Text _timerText, _combatNumberText, _victoryButtonText;
    //displays the time left in the combat, the number of the combat (for player reference) and the text of the button on the victory screen, which will be changed upon beating the last combat 
    [SerializeField] private float _combatTime = 120f; //limits the amount of time a combat can last.
    private float _timer; //if combat isn't over when this hits 0, the combat ends with a draw with neither the player or enemy winning.
    [SerializeField] private GameObject _combatText; //a canvas containing all text in the Combat scene; will be set to not active when CombatActive is false
    [SerializeField] private List<Ship>_combat1EnemyForms, _combat2EnemyForms, _combat3EnemyForms = new();
    //stores the different forms of the enemy ship in all combats; a random enemy form from the respective list, based on the combat number, is spawned each combat.

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } // Checks if another instance already exists and destroys this duplicate
        Instance = this;

        if (_playerShip == null)
        {
            Debug.LogError($"{name}: Player ship missing");
            return;
        }
        if (_victoryScreen == null)
        {
            Debug.LogError($"{name}: Victory screen not set");
            return;   
        }
        if (_defeatScreen == null)
        {
            Debug.LogError($"{name}: Defeat screen not set");
            return;   
        }
        if (_drawScreen == null)
        {
            Debug.LogError($"{name}: Draw screen not set");
            return;   
        }
        if (_timerText == null)
        {
            Debug.LogError($"{name}: Timer text not set");
            return;   
        }
        if (_combatNumberText == null)
        {
            Debug.LogError($"{name}: Combat number text not set"); 
            return;
        }
        if (_combatText == null)
        {
            Debug.LogError($"{name}: Combat number text not set"); 
            return;
        }
        if (_victoryButtonText == null)
        {
            Debug.LogError($"{name}: Victory Button Text not set"); 
            return;
        }
        //bunch of safety precautions as the script needs everything here to be set properly in order to work.

        SpawnEnemyShip(); //calls the function that randomly spawns an enemy, base on the combat number
        if (_enemyShip == null)
        {
            Debug.LogError($"{name}: Enemy ship missing"); 
            return;
        } //if enemy for some reason doesn't spawn, this Debug.LogError will catch it. 

        _playerShip.SetOpposingShip(_enemyShip);
        _enemyShip.SetOpposingShip(_playerShip);
        //calls the SetOpposingShip method for both ships to ensure they attack the rooms in the opposing ship.

        _timer = _combatTime; //sets the _timer to the value of _combatTime; will tick down according to Time.deltaTime.
        _combatNumberText.text = $"Combat {GameManager.Instance.CombatNumber}/3"; //displays the current combat the player is in based on CombatNumber in GameManager.
    }

    private void SpawnEnemyShip()
    {
        List<Ship> enemyForms = null; //creates the list for the forms of the enemy that can be spawned, based on combat number

        if (GameManager.Instance == null)
        {
            Debug.LogError($"{name}: GameManager.Instance is null");
            return;
        } //checks if GameManager exists

        if (GameManager.Instance.CombatNumber == 1)
        {
            enemyForms = _combat1EnemyForms;
        }
        else if (GameManager.Instance.CombatNumber == 2)
        {
            enemyForms = _combat2EnemyForms;
        }
        else if (GameManager.Instance.CombatNumber == 3)
        {
            enemyForms = _combat3EnemyForms;
        } //based on CombatNumber in GameManager, the correct list is found and set to equal enemyForms.
        else
        {
            Debug.LogError($"{name}: Invalid combat number: {GameManager.Instance.CombatNumber}" );
            return;
        } //this shouldn't occur; more of a safety measure in case somehow CombatNumber isn't betwen 1-3.

        if (enemyForms == null || enemyForms.Count == 0)
        {
            Debug.LogError($"{name}: No enemy forms have been assigned for Combat {GameManager.Instance.CombatNumber}");
            return;
        } //if a list is empty, this error appears; again shouldn't happen.

        Ship randomEnemy = enemyForms[Random.Range(0, enemyForms.Count)]; 
        //random index is taken based on the size of enemyForms and set as the enemy that will be spawned.

        if (randomEnemy == null)
        {
            Debug.LogError($"{name}: Random enemy is null");
            return;
        } //if enemyForms had at least one value, this should neevr happen; just for safety reasons.

        Ship enemyShip = Instantiate(randomEnemy);
        enemyShip.transform.position = new Vector2(75, 0);
        _enemyShip = enemyShip;
        //creates the enemy, sets it to its designated position and sets it as the enemy before SetOpposingShip is called for both ships.

        Transform roomsParent = enemyShip.transform.Find("Rooms");
        if (roomsParent == null)
        {
            Debug.LogError($"{enemyShip.name}: Could not find Rooms parent");
            return;
        } //should never happen but is here for safety reasons.

        foreach (Transform roomTransform in roomsParent)
        {
            Room room = roomTransform.GetComponent<Room>();

            if (room != null)
            {
                room.Ship = _enemyShip;
                _enemyShip.AddRoom(room);
            }
        } //usually this is done when each room is placed (this is the case for the Ship Design scene) 
        //but since that doesn't happen in the combat scene, this is where its done instead
    }
    
    void Update()
    {
        if (!CombatActive) return; //checks if combat is supposed to be active
        if(_playerShip.CurrentHealth <= 0 || _enemyShip.CurrentHealth <= 0) EndComat(); //checks if combat should be ending; 
        //either with a win or loss for the player, based on whichever ship, if any, has 0 health

        if(_timer > 0) _timer -= Time.deltaTime;
        else if(_timer <= 0)
        {
            _timer = 0;
            EndComat();
        } //as mentioned earlier, if _timer ever hits 0, combat ends in a draw
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //creates the minutes and seconds that will be displayed and formats the string to properly display those values;
        //in this case always displaying 2 digits for the seconds even if the variable seconds is a single digit.

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero); 
            if (hit.collider == null) return;
            //checks if any collider was hit, using a raycast from the cursor's position.

            Room room = hit.collider.GetComponent<Room>();
            if (room == null) return; //checks if a collider was hit but doesnt have a room script; since this method only applies to objects that have it.
            
            if (room.Ship == _playerShip) //checks if the room has the variable ship, in the Room.cs script, as _playerShip, meaning its a room in the Player Ship.
            {
                Weapon weapon = room.GetComponent<Weapon>();
                if (weapon != null) selectedWeapon = weapon;
                else selectedWeapon = null;
                //this method is meant to select a weapon room and set it as selectedWeapon, so that the player can later set this weapon to target an opposing room. 
            }

            else if (room.Ship == _enemyShip && selectedWeapon != null)
            {
                selectedWeapon.TargetRoom = room;
                selectedWeapon = null;
            } //checks if the room has the variable ship, in the Room.cs script, as _enemyShip, meaning its a room in the Enemy Ship.
            //it then assigns the room as the TargetRoom, in the Weapon.cs script, 
            //for that specfic weapon to target until the player chooses to change it or the weapon room gets destroyed
        }
    }
    private void EndComat() //called when combat should end
    {   
        if (_victoryScreen.activeSelf == true || _defeatScreen.activeSelf == true || _drawScreen.activeSelf == true) return;
        //checks if any of the 3 outcome screens are already active, if they there, this method doesn't have to do anything.
        if (_enemyShip.CurrentHealth <= 0)
        {
            if (GameManager.Instance.CombatNumber == 3) _victoryButtonText.text = "Finish Game";
            //changes the text within the button on the victory screen to tell the player they completed the last combat and can now finish the game.
            
            GameManager.Instance.CombatNumber += 1; //only increases when the player wins to ensure the player can't lose and still move further into the game.
            _victoryScreen.SetActive(true); 
        }
        else if (_playerShip.CurrentHealth <= 0) _defeatScreen.SetActive(true);
        else if (_timer == 0) _drawScreen.SetActive(true);
        CombatActive = false; //sets CombatActive to be false to disable all activity in the Combat scene, as said earlier.
        _combatText.SetActive(false);
    }
    //made with the help of Chat GPT
}
