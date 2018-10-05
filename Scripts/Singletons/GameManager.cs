using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Text levelText;

    public List<EnemyHealth> EnemyList { get; private set; }
    public int RoundKills { get; private set; }

    private float spawnDelay = 1f;
    private float currentSpawnTime = 0f;
    private int currentLevel;

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
        currentLevel = 1;
        currentSpawnTime = 0;
        RoundKills = 0;
        EnemyList = new List<EnemyHealth>();

        StartCoroutine(Spawn());
	}
	
	// Update is called once per frame
	void Update () {
        currentSpawnTime += Time.deltaTime;
	}

    private IEnumerator Spawn() {

        if (currentSpawnTime > spawnDelay) {
            currentSpawnTime = 0;

            if (EnemyList.Count < currentLevel) {
                Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
                Instantiate(enemyToSpawn);
                enemyToSpawn.transform.position = spawnPoint;
            }
        }

        if (RoundKills == EnemyList.Count && EnemyList.Count > 0) {
            EnemyList.Clear();
            RoundKills = 0;
            yield return new WaitForSeconds(3f);
            currentLevel++;
            levelText.text = "Level " + currentLevel;
        }

        yield return null;
        StartCoroutine(Spawn());
    }

    public void PlayerHit(int currentHP) {
        if (currentHP > 0)
            GameOver = false;
        else
            GameOver = true;
    }
}
