using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private Player _player;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] _powerups;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
            
        }
            
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posofPowerup = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);
            if (_player._isShieldActive == false)
            {
                Instantiate(_powerups[UnityEngine.Random.Range(0, 3)], posofPowerup, Quaternion.identity);

            }
            else
            {
                Instantiate(_powerups[UnityEngine.Random.Range(0, 2)], posofPowerup, Quaternion.identity);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));

        }

    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

