using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4.0f;
    private Player playerScript;
    private Animator enemy_anim;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;



    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player not found!");
        }
        enemy_anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the enemy is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        ;

        if (transform.position.y < -5f)
        {
            float randomX = UnityEngine.Random.Range(-8f, 8f);
            transform.position = new UnityEngine.Vector3(randomX, 7, transform.position.z);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            enemy_anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            playerScript.ScoreCal(10);
            enemy_anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
           
        }
            
    }

   
}
