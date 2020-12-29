using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerController : MonoBehaviour
{
    public float MoveSpeed = 0f;

    private Transform _transform;
    private Animator _animator;

    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime);

        // Check if we're out of bounds
        if (_transform.position.x < -20.5f ||
            _transform.position.x > 13.5f ||
            _transform.position.y < -11.5f ||
            _transform.position.y > 15.5f)
        {
            StartCoroutine(Despawn());
        }
    }

    public void SetMoveAngle(float angle)
    {
        _transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void SetMoveSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    public void SetPosition(Vector2 position)
    {
        _transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Obstacle"))
        {
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        _animator.SetTrigger("despawn");
        MoveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
