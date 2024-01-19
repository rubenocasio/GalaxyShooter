using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    
    [SerializeField]
    private GameObject _enemyContainter;

    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    //spawn game objects every 5 seconds
    //create a Coroutine of type IEnumerator == Yield events
    //While loop (infinite game loop)

    IEnumerator SpawnEnemyRoutine()
    {
        //spawn enemies
        //instead of infinite loop to spawn enemies
        //while (true) or below
        while (_stopSpawning == false)
        {
                                                    //x,      y,  z
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);

            // Instantiate or spawn your game object here
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainter.transform;

            //wait for 5 seconds before next loop iteration
            yield return new WaitForSeconds(1f); 
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        //spawn enemies
        //instead of infinite loop to spawn enemies
        //while (true) or below
        while (_stopSpawning == false)
        {
            //x,      y,  z
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);

            int randomPowerUp = UnityEngine.Random.Range(0,3);
            // Instantiate or spawn your game object here
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);

            //wait random seconds before next loop iteration
            yield return new WaitForSeconds(UnityEngine.Random.Range(3,8));
        }

    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
