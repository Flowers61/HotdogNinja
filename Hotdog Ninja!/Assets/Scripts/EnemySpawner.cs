using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject BasicEnemy;

    public float SpawnRateInSeconds = 5f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject newEnemy = (GameObject)Instantiate(BasicEnemy);

        newEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        NextEnemySpawn();
    }

    void NextEnemySpawn()
    {
        float spawnInSeconds;

        if (SpawnRateInSeconds < 1f)
        {
            spawnInSeconds = Random.Range(1f, SpawnRateInSeconds);

        }

        else
            spawnInSeconds = 1f;

        Invoke("SpawnEnemy", spawnInSeconds);
    }

    void IncreaseSpawnRate()
    {
        if (SpawnRateInSeconds > 1f)
            SpawnRateInSeconds--;
        if (SpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    public void ScheduleEnemySpawner()
    {
        Invoke("SpawnEnemy", SpawnRateInSeconds);

        InvokeRepeating("IncreaseSpawnRate", 0f, 20f);
    }
    public void UnscheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpwanRate");
    }
}
