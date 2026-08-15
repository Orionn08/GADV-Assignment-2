using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
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

    private void Awake()
    {
        Instance = this;

        if (_playerShip == null)
        {
            Debug.LogError($"{name}: Player ship missing");
            return;
        }
        if (_enemyShip == null)
        {
            Debug.LogError($"{name}: Enemy ship missing");
            return;
        }
        _playerShip.SetOpposingShip(_enemyShip);
        _enemyShip.SetOpposingShip(_playerShip);

        _timer = _combatTime;
    }

    void Update()
    {   
        if(_playerShip.currentHealth == 0 || _enemyShip.currentHealth == 0) EndComat();

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

            else if (room.ship == _enemyShip)
            {
                if (_selectedWeapon != null) _selectedWeapon.targetRoom = room;
            }
        }
    }
    private void EndComat()
    {   
        if (_victoryScreen.activeSelf == true || _defeatScreen.activeSelf == true || _drawScreen.activeSelf == true) return;
        if (_enemyShip.currentHealth == 0) _victoryScreen.SetActive(true);
        if (_playerShip.currentHealth == 0) _defeatScreen.SetActive(true);
        if (_timer == 0) _drawScreen.SetActive(true);
        CombatActive = false;
        _timerText.gameObject.SetActive(false);
    }
    //made with the help of Chat GPT
}
