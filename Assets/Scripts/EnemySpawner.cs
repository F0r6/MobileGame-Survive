using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public bool spawnCooldown = false;
    private static float timerReset = 0.2f;
    private float coolDownTimer = timerReset;

    Vector3 RandRange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RandRange = (Vector3)Random.insideUnitCircle.normalized * Random.Range(30, 31);
        ApplySpawnCooldown();
    }

    void SpawnEnemy()
    {
        int rand = Random.Range(1, 2);
        if (rand == 1)
        {
            spawnCooldown = true;
            Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        }
    }

    void ApplySpawnCooldown()
    {
        if (spawnCooldown)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if(coolDownTimer < 0.0f)
        {
            spawnCooldown = false;

            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + RandRange;

            SpawnEnemy();
            
            coolDownTimer = timerReset;
        }
        else
        {
            spawnCooldown = true;
        }
    }
}
