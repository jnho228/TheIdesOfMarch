using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject daggerObject;

    private Animator anim;

    private float daggerThrowTimer = 1f;
    private float daggerThrowDelay = 1f;

    private float moveAngle = 0;
    private float moveSpeed = 0;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        daggerThrowTimer = Random.Range(0.5f, 2.5f);
        daggerThrowDelay = Random.Range(0.5f, 2.5f);
    }

    void Update()
    {
        Vector2 movement = new Vector2(Mathf.Sin(moveAngle), Mathf.Cos(moveAngle) * -1);

        myTransform.Translate(movement.normalized * moveSpeed * Time.deltaTime);

        // Check if we're out of bounds
        if (myTransform.position.x < -20.5f ||
            myTransform.position.x > 13.5f ||
            myTransform.position.y < -11.5f ||
            myTransform.position.y > 15.5f)
        {
            StartCoroutine(Despawn());
        }

        // Dagger spawn timer
        if (daggerThrowTimer < Time.deltaTime)
        {
            DaggerController dc = Instantiate(daggerObject, myTransform.position, Quaternion.identity).GetComponent<DaggerController>();
            dc.SetMoveAngle(Random.Range(0, 360));
            dc.SetMoveSpeed(Random.Range(2.5f, 3.5f));

            daggerThrowTimer = daggerThrowDelay + Random.Range(-0.3f, 0.3f);
        }
        else
            daggerThrowTimer -= Time.deltaTime;

    }

    public void SetMoveAngle(float angle)
    {
        moveAngle = angle * Mathf.Deg2Rad;
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
        if (collision.CompareTag("Obstacle"))
        {
            SetMoveAngle(Random.Range(0, 360)); // Change this to see the point they hit and bounce them lol
        }
    }

    IEnumerator Despawn()
    {
        moveSpeed = 0;
        anim.SetTrigger("despawn");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
