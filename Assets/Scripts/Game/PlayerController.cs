using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Todo List:
 * [ ] Dash to avoid enemies.
 * [x] Animator for movement and stun and death
 */

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public LayerMask MovementCollisionLayerMask;

    private bool IsAlive => StabCount < 23;
    private int StabCount = 0;
    private bool IsStunned = false;

    private Transform _transform;
    private AudioSource _audioSource;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _capsuleCollider2D;
    private float _stunTimer = 0.5f;
    private readonly float _stunTimerDelay = 0.5f;

    [SerializeField] private UnityEvent OnPlayerHit, OnPlayerDeath;

    void Awake()
    {
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (!IsAlive)
            return;
        
        if (IsStunned)
        {
            _stunTimer -= Time.deltaTime;

            if (_stunTimer < Time.deltaTime)
                IsStunned = false;
            
            return;
        }

        Movement();
    }

    void Movement()
    {
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity = velocity.normalized * MoveSpeed;

        RaycastHit2D hit = Physics2D.Raycast(_transform.position, velocity, 0.5f, MovementCollisionLayerMask);
        if (hit)
            velocity = Vector2.zero;

        _animator.Play( (velocity == Vector2.zero) ? "Idle" : "Walk");

        if (velocity.x != 0) // Only flip the sprite and collider if we're moving horizontal. Up/down/stationery we just keep orientation.
        {
            _spriteRenderer.flipX = velocity.x < 0; 
            _capsuleCollider2D.offset *= new Vector2(velocity.x < 0 ? -1 : 1, 1);
        }

        _transform.Translate(velocity * Time.deltaTime);

        //Keep me locked in between x: -19.5 and 12.5 / y: 14.5 and -10.5
        _transform.position = new Vector2(Mathf.Clamp(_transform.position.x, -19.5f, 12.5f), Mathf.Clamp(_transform.position.y, -10.5f, 14.5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Dagger"))
        {
            GetStabbed();
            ScreenShake.Instance.ShakeCamera(5.5f, .4f);
        }
    }

    private void GetStabbed()
    {
        OnPlayerHit.Invoke();
        StabCount++;

        _audioSource.Play();
        _animator.Play("Hit");

        if (!IsAlive)
        {
            _animator.Play("Die");
            OnPlayerDeath.Invoke();
        }
        else
        {
            // Stun the player.
            IsStunned = true;
            _stunTimer = _stunTimerDelay;

        }
    }
}
