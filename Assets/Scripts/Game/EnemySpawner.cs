using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float MIN_MOVE_SPEED = 1f;
    private const float MAX_MOVE_SPEED = 4f;
    private const float SPAWN_ANGLE_VARIATION = 15f;

    public GameDifficulty gameDifficulty;
    public GameObject enemyObject;
    public GameObject daggerObject;

    private bool IsActive = true;

    private float _spawnTimer = 1f;
    private readonly float _spawnDelay = 5f;
    private float _spawnIncreaseTimer = 5f;
    private readonly float _spawnIncreaseDelay = 10f;
    private int _spawnRateLevel = 0;
    private float _gameDifficultyIncreaseTimer = 60f;
    private readonly float _gameDifficultyIncreaseDelay = 60f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //remove this later
            Application.Quit(-10);

        if (!IsActive)
            return;

        // Timer stuff
        if (_spawnTimer < Time.deltaTime)
        {
            // Spawn stuff
            // Define a playing area and have mobs spawn on the edge and move randomly in a direction

            //-20.5, 13.5 / -11.5, 15.5
            EnemyController newEnemy = Instantiate(enemyObject, new Vector2(0, 0), Quaternion.identity).GetComponent<EnemyController>();
            int randomSide = Random.Range(0, 4);

            if (randomSide == 0) //left
            {
                newEnemy.SetPosition(new Vector2(-20.5f, Random.Range(-10.5f, 14.5f)));
                newEnemy.SetMoveAngle(90 + Random.Range(-SPAWN_ANGLE_VARIATION, SPAWN_ANGLE_VARIATION));
                newEnemy.SetMoveSpeed(Random.Range(MIN_MOVE_SPEED, MAX_MOVE_SPEED));
            }
            if (randomSide == 1) //top
            {
                newEnemy.SetPosition(new Vector2(Random.Range(-19.5f, 12.5f), 15.5f));
                newEnemy.SetMoveAngle(Random.Range(-SPAWN_ANGLE_VARIATION, SPAWN_ANGLE_VARIATION));
                newEnemy.SetMoveSpeed(Random.Range(MIN_MOVE_SPEED, MAX_MOVE_SPEED));
            }
            if (randomSide == 2) //right
            {
                newEnemy.SetPosition(new Vector2(13.5f, Random.Range(-10.5f, 14.5f)));
                newEnemy.SetMoveAngle(-90 + Random.Range(-SPAWN_ANGLE_VARIATION, SPAWN_ANGLE_VARIATION));
                newEnemy.SetMoveSpeed(Random.Range(MIN_MOVE_SPEED, MAX_MOVE_SPEED));
            }
            if (randomSide == 3) //bottom
            {
                newEnemy.SetPosition(new Vector2(Random.Range(-19.5f, 12.5f), -11.5f));
                newEnemy.SetMoveAngle(180 + Random.Range(-SPAWN_ANGLE_VARIATION, SPAWN_ANGLE_VARIATION));
                newEnemy.SetMoveSpeed(Random.Range(MIN_MOVE_SPEED, MAX_MOVE_SPEED));
            }

            _spawnTimer = _spawnDelay - (_spawnRateLevel * 0.5f);

            if (_spawnTimer < 0.5f)
                _spawnTimer = 0.5f;
        }
        else
            _spawnTimer -= Time.deltaTime;

        // Difficulty Increase
        if (_spawnIncreaseTimer < Time.deltaTime)
        {
            // I think with each difficulty bump I wanna spawn some sort of super enemy lol

            _spawnRateLevel++;
            _spawnIncreaseTimer = _spawnIncreaseDelay;
        }
        else
            _spawnIncreaseTimer -= Time.deltaTime;

        if (_gameDifficultyIncreaseTimer < Time.deltaTime)
        {
            gameDifficulty.Difficulty++;
            _gameDifficultyIncreaseTimer = _gameDifficultyIncreaseDelay;
        }
        else
            _gameDifficultyIncreaseTimer -= Time.deltaTime;
    }

    public void EndGame()
    {
        IsActive = false;

        // Pause everything
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject i in enemies)
        {
            EnemyController enemyController = i.GetComponent<EnemyController>();
            enemyController.enabled = false;
        }

        GameObject[] daggers = GameObject.FindGameObjectsWithTag("Dagger");

        foreach(GameObject i in daggers)
        {
            DaggerController daggerController = i.GetComponent<DaggerController>();
            daggerController.enabled = false;
        }
    }
}
