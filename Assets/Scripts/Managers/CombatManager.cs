using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public bool CombatActive { get; private set; } = true;
    [SerializeField] private Ship _playerShip;
    [SerializeField] private Ship _enemyShip;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _defeatScreen;

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
    }

    void Update()
    {   
        if(_playerShip.currentHealth == 0 || _enemyShip.currentHealth == 0) EndComat();
    }
    private void EndComat()
    {   
        if (_victoryScreen.activeSelf == true || _defeatScreen.activeSelf == true) return;
        if (_enemyShip.currentHealth == 0) _victoryScreen.SetActive(true);
        if (_playerShip.currentHealth == 0) _defeatScreen.SetActive(true);
        CombatActive = false;
    }
    //made with the help of Chat GPT
}
