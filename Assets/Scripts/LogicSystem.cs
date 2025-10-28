using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class LogicSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject scoreScreen;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI yourScoreText;
    public GameObject playAgainScreen;

    [Header("Bomb Spawning")]
    public bool bombsEnabled = true;
    public int spawnChance = 30;
    public int maxTickSpacing = 50;
    public float zRange = 8;
    public float xOffset = 25;
    public float xRange = 5;

    [Header("Bomb Targeting")]
    public Vector3 targetOffset = new Vector3(0, 0, 0);
    private int currentScore;
    private int bombTicks = 0;
    private PlayerController playerScript;

    [Header("Optimization")]
    public bool useObjectPooling = true;

    private bool gameOverSkipped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = player.transform.GetComponent<PlayerController>();
        spawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();

        if (!gameOverSkipped && !playerScript.isAlive && playerScript.GetComponent<PlayerInput>().actions["submit"].WasPressedThisFrame())
        {
            gameOverSkipped = true;
            StopAllCoroutines();
            gameOverText.gameObject.SetActive(false);
            scoreScreen.SetActive(false);
            scoreScreen.SetActive(false);
            playAgainScreen.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        int number = Random.Range(0, spawnChance);
        bombTicks++;
        if ((number == 1 | bombTicks == maxTickSpacing) && playerScript.isAlive && bombsEnabled)
        {
            bombTicks = 0;
            spawnEnemy();
        }
    }

    private void updateScore()
    {
        if (player.transform.position.x + 24 > currentScore)
        {
            currentScore = (int)(player.transform.position.x + 24);
        }
        scoreText.text = currentScore.ToString();
    }

    public void gameOver()
    {
        if (currentScore > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", currentScore);
        }

        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
        yourScoreText.text = currentScore.ToString();

        StartCoroutine(gameOverScreen());
    }

    public IEnumerator gameOverScreen()
    {
        scoreText.gameObject.SetActive(false);
        StartCoroutine(flash(4, gameOverText.gameObject));
        yield return new WaitForSeconds(4);
        StartCoroutine(flash(4, scoreScreen));
        yield return new WaitForSeconds(4);
        scoreScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        scoreScreen.gameObject.SetActive(false);
        playAgainScreen.SetActive(true);
    }

    public void reload()
    {
        SceneManager.LoadScene("Main");
    }

    public void spawnEnemy()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 spawnPos = new Vector3(player.transform.position.x + xOffset + Random.Range(-xRange, xRange), playerPos.y, Random.Range(-zRange, zRange));
        GameObject enemy = ObjectPool.SharedInstance.InstantiateFromPool(spawnPos, Quaternion.identity);
        if (enemy != null) {
            enemy.GetComponent<BombScript>().targetPosition = new Vector3(targetOffset.x + playerPos.x + (Mathf.Round(playerScript.inputVector.y) * playerScript.moveSpeed * 1.33f), targetOffset.y + playerPos.y, targetOffset.z + playerPos.z);
            enemy.GetComponent<BombScript>().Launch();
        }
    }

    public IEnumerator flash(int times, GameObject item)
    {
        for (var i = 0; i < times; i++)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            item.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public int getScore() {
        return currentScore;
    }
}
