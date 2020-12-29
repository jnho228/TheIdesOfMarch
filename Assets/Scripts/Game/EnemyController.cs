using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ToDo List
 * [ ] Fix spawning and despawning animations. Since I'm directly calling them and not using the built in animation stuff, it's wonky.
 */

public class EnemyController : MonoBehaviour
{
    public GameDifficulty gameDifficulty;
    public GameObject daggerObject;

    private bool IsAttacking => _attackTimer > 0;

    private Transform _transform;
    private Animator _animator;
    private AudioSource _enemyAttackAudioSource;
    private SpriteRenderer _spriteRenderer;
    private float _daggerThrowTimer = 1f;
    private float _daggerThrowDelay = 1f;
    private float _moveAngle = 0;
    private float _moveSpeed = 0;
    private float _attackTimer = 0f;
    private readonly float _attackDelay = 1f;

    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _enemyAttackAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _daggerThrowTimer = Random.Range(0.5f, 2.5f);
        _daggerThrowDelay = Random.Range(0.5f, 2.5f);
    }

    void Update()
    {
        if (IsAttacking)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }

        Vector2 velocity = new Vector2(Mathf.Sin(_moveAngle), Mathf.Cos(_moveAngle) * -1);

        _transform.Translate(velocity.normalized * _moveSpeed * Time.deltaTime);

        _animator.Play((velocity == Vector2.zero) ? "idle" : "walk");

        // Flip us if we're moving! 
        if (velocity.x != 0)
            _spriteRenderer.flipX = velocity.x < 0;

        // Check if we're out of bounds
        if (_transform.position.x < -20.5f ||
            _transform.position.x > 13.5f ||
            _transform.position.y < -11.5f ||
            _transform.position.y > 15.5f)
        {
            StartCoroutine(Despawn());
        }

        // Dagger spawn timer
        if (_daggerThrowTimer < Time.deltaTime)
        {
            int spawnAmount = Random.Range(1, gameDifficulty.Difficulty + 1);

            for (int i = 0; i < spawnAmount; i++)
            {
                DaggerController daggerController = Instantiate(daggerObject, _transform.position, Quaternion.identity).GetComponent<DaggerController>();
                daggerController.SetMoveAngle(Random.Range(0, 360));
                daggerController.SetMoveSpeed(Random.Range(2.5f, 3.5f));
            }

            _enemyAttackAudioSource.Play();
            _daggerThrowTimer = _daggerThrowDelay + Random.Range(-0.3f, 0.3f);
        }
        else
            _daggerThrowTimer -= Time.deltaTime;

    }

    public void SetMoveAngle(float angle)
    {
        _moveAngle = angle * Mathf.Deg2Rad;
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetPosition(Vector2 position)
    {
        _transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            SetMoveAngle(Random.Range(0, 360)); // Change this to see the point they hit and bounce them lol
        }

        if (collision.CompareTag("Player"))
        {
            // Trigger an attack! :)
            _attackTimer = _attackDelay;
            _animator.Play("attack");
        }
    }

    IEnumerator Despawn()
    {
        _moveSpeed = 0;
        _animator.SetTrigger("despawn");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnDisable() // Just test if this works?
    {
        _animator.enabled = false;
    }
}
