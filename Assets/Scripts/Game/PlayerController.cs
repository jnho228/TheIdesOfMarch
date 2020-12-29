using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Todo List:
 * [ ] Dash to avoid enemies.
 */

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public LayerMask CollisionLayerMask;

    private Transform _transform;
    private AudioSource _audioSource;
    private int StabCount = 0;
    private bool IsAlive => StabCount < 23;

    [SerializeField] private UnityEvent OnPlayerHit, OnPlayerDeath;

    void Awake()
    {
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsAlive)
            return;

        Movement();
    }

    void Movement()
    {
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity = velocity.normalized * MoveSpeed;

        RaycastHit2D hit = Physics2D.Raycast(_transform.position, velocity, 0.5f, CollisionLayerMask);
        if (hit)
            velocity = Vector2.zero;

        _transform.Translate(velocity * Time.deltaTime);

        //Keep me locked in between x: -19.5 and 12.5 / y: 14.5 and -10.5
        _transform.position = new Vector2(Mathf.Clamp(_transform.position.x, -19.5f, 12.5f), Mathf.Clamp(_transform.position.y, -10.5f, 14.5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Dagger"))
        {
            OnStabbed();
            ScreenShake.Instance.ShakeCamera(5f, .5f);
        }
    }

    private void OnStabbed()
    {
        OnPlayerHit.Invoke();
        StabCount++;

        _audioSource.Play();

        if (StabCount >= 23)
            OnPlayerDeath.Invoke();
    }
}
