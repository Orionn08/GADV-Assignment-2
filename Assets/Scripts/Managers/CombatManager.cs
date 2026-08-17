using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Security.Cryptography;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public bool CombatActive { get; private set; } = true;
    [SerializeField] private Ship _playerShip;
    [SerializeField] private Ship _enemyShip;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _defeatScreen;
    [SerializeField] private GameObject _drawScreen;
    private Weapon _selectedWeapon;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _combatTime = 120f;
    private float _timer;
    [SerializeField] private TMP_Text _combatNumberText;
    [SerializeField] private List<Ship>_combat1EnemyForms = new();
    [SerializeField] private List<Ship>_combat2EnemyForms = new();
    [SerializeField] private List<Ship>_combat3EnemyForms = new();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

        SpawnEnemyShip();
        if (_enemyShip == null)
        {
            Debug.LogError($"{name}: Enemy ship missing");
            return;
        }

        _playerShip.SetOpposingShip(_enemyShip);
        _enemyShip.SetOpposingShip(_playerShip);

        _timer = _combatTime;
        _combatNumberText.text = $"Combat {GameManager.Instance.CombatNumber}/3";
    }

    private void SpawnEnemyShip()
    {
        List<Ship> enemyForms = null;

        if (GameManager.Instance == null)
        {
            Debug.LogError($"{name}: GameManager.Instance is null");
            return;
        }

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
        }
        else
        {
            Debug.LogError($"{name}: Invalid combat number: {GameManager.Instance.CombatNumber}" );
            return;
        }

        if (enemyForms == null || enemyForms.Count == 0)
        {
            Debug.LogError($"{name}: No enemy forms have been assigned for Combat {GameManager.Instance.CombatNumber}");
            return;
        }

        Ship randomEnemy = enemyForms[Random.Range(0, enemyForms.Count)];

        if (randomEnemy == null)
        {
            Debug.LogError($"{name}: Random enemy is null");
            return;
        }

        Ship enemyShip = Instantiate(randomEnemy);
        enemyShip.transform.position = new Vector2(75, 0);
        _enemyShip = enemyShip;

        Transform roomsParent = enemyShip.transform.Find("Rooms");
        if (roomsParent == null)
        {
            Debug.LogError($"{enemyShip.name}: Could not find Rooms parent");
            return;
        }

        foreach (Transform roomTransform in roomsParent)
        {
            Room room = roomTransform.GetComponent<Room>();

            if (room != null)
            {
                room.ship = _enemyShip;
                _enemyShip.AddRoom(room);
            }
        }
    }
    void Update()
    {
        if (!CombatActive) return;
        if(_playerShip.currentHealth <= 0 || _enemyShip.currentHealth <= 0) EndComat();

        if(_timer > 0) _timer -= Time.deltaTime;
        else if(_timer <= 0)
        {
            _timer = 0;
            EndComat();
        }
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null) return;

            Room room = hit.collider.GetComponent<Room>();
            if (room == null) return;
            
            if (room.ship == _playerShip)
            {
                Weapon weapon = room.GetComponent<Weapon>();
                if (weapon != null) _selectedWeapon = weapon;
                else _selectedWeapon = null;
            }

            else if (room.ship == _enemyShip) if (_selectedWeapon != null) _selectedWeapon.targetRoom = room;
        }
    }
    private void EndComat()
    {   
        if (_victoryScreen.activeSelf == true || _defeatScreen.activeSelf == true || _drawScreen.activeSelf == true) return;
        if (_enemyShip.currentHealth <= 0)
        {
            GameManager.Instance.CombatNumber += 1; _victoryScreen.SetActive(true);
        }
        if (_playerShip.currentHealth <= 0) _defeatScreen.SetActive(true);
        if (_timer == 0) _drawScreen.SetActive(true);
        CombatActive = false;
        _timerText.gameObject.SetActive(false);
        _combatNumberText.gameObject.SetActive(false);
    }
    //made with the help of Chat GPT
}
