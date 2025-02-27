using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float minSpawnDist = 25f;

    [SerializeField]
    GameObject enemy;
    [SerializeField]
    GameObject boss;

    [SerializeField]
    int enemiesToSpawn = 10;

    [SerializeField]
    Transform pivot;

    [SerializeField]
    float timeBetweenWave = 15f;
    float time;
    int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        waveCount = 0;
        //SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= timeBetweenWave) {
            SpawnWave();
            time = 0;
            //(Changed) timeBetweenWave decreases with each wave
            timeBetweenWave /= 1.01f;
        }
    }

    void SpawnWave()
    {
        //(Changed) spawns at a random point that is minSpawnDist away from (0,0) instead of at (0, 25)
        System.Random rand = new System.Random();
        float x = (float)rand.NextDouble() * 2f - 1f;
        //(Changed) y is calculated with Math.sqrt(1-Math.Pow(x,2f)) so the vector is already normalized
        //aside for some floating point arithmetic errors
        Vector2 spawnPos = new Vector2(x,
            (float)(2*(Math.Sqrt(1 - Math.Pow(x, 2f)))-1)
            ) * minSpawnDist;
        print("("+ spawnPos.x+","+spawnPos.y+")");
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject spawnObject = Instantiate(enemy, new Vector3(spawnPos.x,spawnPos.y), Quaternion.identity);
            //spawnObject.GetComponent<basicEnemy>();
        }
        waveCount++;
        if(waveCount % 5==0) Instantiate(boss, new Vector3(spawnPos.x, spawnPos.y), Quaternion.identity);
    }
}
