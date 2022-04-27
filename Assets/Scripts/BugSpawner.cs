using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private Transform bugEnemyPrefab;
    private float _cooldown = 5f;

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0)
        {
            int randInt = Random.Range(0, 2);
            
            var enemy = Instantiate(bugEnemyPrefab);

            if (randInt == 0)
            {
                enemy.GetComponent<BugEnemy>().InitializeWalk();
            }
            else
            {
                enemy.GetComponent<BugEnemy>().InitializeFly();
            }

            _cooldown = 5f;
        }
    }
}
