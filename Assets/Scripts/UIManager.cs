using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    private Player playerScript;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    private float _flickerSpeed = 0.5f;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    [SerializeField]
    private GameManager _gameManager;

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
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL!");
        }
        _scoreText.text = "Score: " + 0;

        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + playerScript._score.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _livesSprites[currentLives];
    }
    public void GameOver()
    {
        StartCoroutine(GameOVerFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

    }

    IEnumerator GameOVerFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_flickerSpeed);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerSpeed);

        }
    }
}
