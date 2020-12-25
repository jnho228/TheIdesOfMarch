using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerController : MonoBehaviour
{
    public float moveSpeed = 0f;

    private Transform myTransform;
    private Animator anim;

    private void Awake()
    {
        myTransform = transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        myTransform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

        // Check if we're out of bounds
        if (myTransform.position.x < -20.5f ||
            myTransform.position.x > 13.5f ||
            myTransform.position.y < -11.5f ||
            myTransform.position.y > 15.5f)
        {
            StartCoroutine(Despawn());
        }
    }

    public void SetMoveAngle(float angle)
    {
        myTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetPosition(Vector2 position)
    {
        myTransform.position = position;
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
        anim.SetTrigger("despawn");
        moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
