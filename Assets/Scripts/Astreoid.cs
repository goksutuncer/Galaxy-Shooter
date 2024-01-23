using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astreoid : MonoBehaviour
{
    private float _rotationSpeed = 5f;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
 

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);
            GameObject new_explosion = Instantiate(_explosion,transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
            


        }

    }

}
