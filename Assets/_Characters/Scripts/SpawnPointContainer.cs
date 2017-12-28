using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointContainer : MonoBehaviour {

    GameObject [] Spawnpoints;
    [SerializeField] GameObject [] enemiesToSpawn;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] int maxEnemySpawned = 5;
    int enemiesSpawned = 0;


	// Use this for initialization
	void Awake () {
        Spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
	}

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            if(enemiesSpawned < maxEnemySpawned)
            {
                var randomSpawn = Random.Range(0, Spawnpoints.Length);
                var randomEnemy = Random.Range(0, enemiesToSpawn.Length);
                Instantiate(enemiesToSpawn[randomEnemy], Spawnpoints[randomSpawn].transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    // Update is called once per frame
    void Update () {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesSpawned = enemies.Length;
	}

    public GameObject[] spawnPoints()
    {
        return Spawnpoints;
    }
}
