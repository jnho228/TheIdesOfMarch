using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameDifficulty gameDifficulty;
    public GameObject daggerObject;

    private Transform _transform;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _daggerThrowTimer = 1f;
    private float _daggerThrowDelay = 1f;
    private float _moveAngle = 0;
    private float _moveSpeed = 0;

    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _daggerThrowTimer = Random.Range(0.5f, 2.5f);
        _daggerThrowDelay = Random.Range(0.5f, 2.5f);
    }

    void Update()
    {
        Vector2 movement = new Vector2(Mathf.Sin(_moveAngle), Mathf.Cos(_moveAngle) * -1);

        _transform.Translate(movement.normalized * _moveSpeed * Time.deltaTime);

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
                DaggerController dc = Instantiate(daggerObject, _transform.position, Quaternion.identity).GetComponent<DaggerController>();
                dc.SetMoveAngle(Random.Range(0, 360));
                dc.SetMoveSpeed(Random.Range(2.5f, 3.5f));
            }

            _audioSource.Play();
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
    }

    IEnumerator Despawn()
    {
        _moveSpeed = 0;
        _animator.SetTrigger("despawn");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
