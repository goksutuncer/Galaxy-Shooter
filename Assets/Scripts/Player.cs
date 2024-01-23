using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 3;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    public bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    public int _score = 0;

    private UIManager _uIManager;
    [SerializeField]
    private GameObject[] _EngineFires;
    [SerializeField]
    private AudioClip _LaserSoundClip;
    private AudioSource _audioSource;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new UnityEngine.Vector3(0, 0, 0);
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        { 
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");  
        }
        else
        {
            _audioSource.clip = _LaserSoundClip;
        }

        foreach (GameObject obj in _EngineFires)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }
    void CalculateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        UnityEngine.Vector3 direction = new UnityEngine.Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new UnityEngine.Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2.9f, 0), transform.position.z);

        if (transform.position.x >= 9.3f)
        {
            transform.position = new UnityEngine.Vector3(-9.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -9.3f)
        {
            transform.position = new UnityEngine.Vector3(9.3f, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position, UnityEngine.Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new UnityEngine.Vector3(0, 1.05f, 0), UnityEngine.Quaternion.identity);
        }
        _audioSource.Play();
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.gameObject.SetActive(false);
            return;
        }
        _lives--;
        _uIManager.UpdateLives(_lives);
        if (_lives == 2)
        {
            _EngineFires[Random.Range(0, 2)].SetActive(true);
        }
        else if (_lives == 1)
        {
            foreach (GameObject obj in _EngineFires)
            {
                obj.SetActive(true);
            }
        }
        else if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uIManager.GameOver();
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);

    }
    public void ScoreCal(int points)
    {
        _score += points;
    }
}



