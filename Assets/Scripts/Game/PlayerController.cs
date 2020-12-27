using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Todo List:
 * [ ] Dash to avoid enemies.
 */

public class PlayerController : MonoBehaviour
{
    public GameGUI gameGUI;
    public float moveSpeed = 5f;

    public LayerMask collisionLayerMask;

    private Transform myTransform;
    private AudioSource audioSource;
    private int stabCount = 0;
    private bool isAlive = true;

    void Awake()
    {
        myTransform = transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isAlive)
            return;

        Movement();
    }

    void Movement()
    {
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity = velocity.normalized * moveSpeed;

        RaycastHit2D hit = Physics2D.Raycast(myTransform.position, velocity, 0.5f, collisionLayerMask);

        if (hit)
            velocity = Vector2.zero;

        myTransform.Translate(velocity * Time.deltaTime);

        //Keep me locked in between x: -19.5 and 12.5 / y: 14.5 and -10.5
        myTransform.position = new Vector2(Mathf.Clamp(myTransform.position.x, -19.5f, 12.5f), Mathf.Clamp(myTransform.position.y, -10.5f, 14.5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Dagger"))
        {
            OnStabbed();
        }
    }

    private void OnStabbed()
    {
        gameGUI.AddStab();
        stabCount++;

        //FindObjectOfType<AudioManager>().Play("PLAYER_HIT");
        audioSource.Play();

        if (stabCount >= 23)
            GameOver();
    }

    void GameOver()
    {
        gameGUI.ShowGameOver();
        //pause the spawning
        isAlive = false;
    }
}
