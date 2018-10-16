using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private GameObject player;

    // Variables related to Enemy Spawning
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    public List<EnemyHealth> EnemyList { get; private set; }
    private float spawnDelay = 1f;
    private float currentSpawnTime = 0f;

    // Variables related to Power Up Spawning
    [SerializeField] private Transform[] powerUpSpawns;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private int maxPowerUps = 4;
    [SerializeField] private float powerUpSpawnDelay = 5f;
    private float currentPowerUpSpawnTime = 0f;
    private int numberOfPowerUps = 0;

    // In Game HUD and stats
    [SerializeField] private Text levelText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text endGameText;
    [SerializeField] private AudioClip defeat;
    [SerializeField] private int finalLevel = 20;
    private float timeInGame;
    private int currentLevel;

    // Public Getters and Setter
    public int RoundKills { get; private set; }
    public bool GameOver { get; private set; }
    public GameObject Player { get { return player; } }

    public void RegisterEnemy(EnemyHealth enemy) {
        EnemyList.Add(enemy);
    }

    public void RegisterKill() {
        RoundKills++;
    }

    private void Awake() {
        Assert.IsNotNull(spawnPoints);
        Assert.IsNotNull(enemies);
        Assert.IsNotNull(levelText);
    }

    // Use this for initialization
    void Start () {
        endGameText.enabled = false;
        timeInGame = 0;

        currentLevel = 1;
        currentSpawnTime = 0;
        RoundKills = 0;
        EnemyList = new List<EnemyHealth>();

        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

	// Update is called once per frame
	void Update () {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
        timeInGame += Time.deltaTime;
        
        timerText.text = string.Format("{0:#0}:{1:0#}", Mathf.FloorToInt(timeInGame / 60), Mathf.FloorToInt(timeInGame % 60));
	}

    private IEnumerator SpawnEnemy() {

        if (currentSpawnTime > spawnDelay) {
            currentSpawnTime = 0;

            if (EnemyList.Count < currentLevel) {
                Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)]; 
                Instantiate(enemyToSpawn);
                enemyToSpawn.transform.position = spawnPoint;
            }
        }

        if (RoundKills == EnemyList.Count && EnemyList.Count > 0 && currentLevel != finalLevel) {
            EnemyList.Clear();
            RoundKills = 0;
            yield return new WaitForSeconds(3f);
            currentLevel++;
            levelText.text = "Level " + currentLevel;
        }

        if (RoundKills == finalLevel) {
            StartCoroutine(EndGame("Victory!"));
        }

        yield return null;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnPowerUp() {

        if (currentPowerUpSpawnTime > powerUpSpawnDelay) {
            currentPowerUpSpawnTime = 0;

            if (numberOfPowerUps < maxPowerUps) {
                // get random of the two spots
                Transform spawnArea = powerUpSpawns[Random.Range(0, powerUpSpawns.Length)];

                // choose a random point from the bounds of the area
                // will use a box collider for this 'bounds'
                BoxCollider box = spawnArea.gameObject.GetComponent<BoxCollider>();
                Vector3 spawnPoint = new Vector3(Random.Range(box.bounds.min.x, box.bounds.max.x), spawnArea.position.y, Random.Range(box.bounds.min.z, box.bounds.max.z));

                // select a random powerup to spawn
                GameObject powerUpToSpawn = powerUps[Random.Range(0, powerUps.Length)];

                Instantiate(powerUpToSpawn);
                numberOfPowerUps++;
                powerUpToSpawn.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z);
            }
        }

        yield return null;
        StartCoroutine(SpawnPowerUp());
    }

    public void PlayerHit(int currentHP) {
        if (currentHP > 0)
            GameOver = false;
        else {
            GameOver = true;
            StartCoroutine(EndGame("Defeat"));
        }
    }

    IEnumerator EndGame(string result) {

        if (GameOver)
            GetComponent<AudioSource>().PlayOneShot(defeat);
        // else play victory

        yield return new WaitForSeconds(1f);
        endGameText.text = result;
        endGameText.enabled = true;
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Main Menu");
    }
}
