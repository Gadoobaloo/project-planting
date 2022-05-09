using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private Transform bugEnemyPrefab;

    [SerializeField] private float xLocation;
    [SerializeField] private float yLocationMin;
    [SerializeField] private float yLocationMax;

    private float _cooldown = 5f;

    private void Start()
    {
        ChangePosition();
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0)
        {
            int randInt = Random.Range(0, 2);

            var enemy = Instantiate(bugEnemyPrefab, transform.position, Quaternion.identity);

            if (randInt == 0)
            {
                enemy.GetComponent<BugEnemy>().InitializeWalk();
            }
            else
            {
                enemy.GetComponent<BugEnemy>().InitializeFly();
            }

            ChangePosition();
            _cooldown = Random.Range(5f, 20f);
        }
    }

    private void ChangePosition()
    {
        float yLocation = Random.Range(yLocationMin, yLocationMax);
        transform.position = new Vector3(xLocation, yLocation, 0f);
    }
}