using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Ship playerShip;
    [SerializeField] private Ship enemyShip;

    private void Awake()
    {
        if (playerShip == null)
        {
            Debug.LogError($"{name}: Player ship missing");
            return;
        }
        if (enemyShip == null)
        {
            Debug.LogError($"{name}: Enemy ship missing");
            return;
        }
        playerShip.SetOpposingShip(enemyShip);
        enemyShip.SetOpposingShip(playerShip);
    }
    //made with the help of Chat GPT
}
