using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public int totalEnemies;
    public int enemiesLeft;

    private void Start()
    {
        TotalEnemiesMethod();
        enemiesLeft = totalEnemies;
    }

    void Update()
    {
        EnemiesLeftMethod();
    }

    public void TotalEnemiesMethod()
    {
        foreach (var abc in GetComponentsInChildren<Enemystuff>())
        {
            totalEnemies -= 1;
        }
    }

    public void EnemiesLeftMethod()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            enemiesLeft -= 1;
        }
    }
}
