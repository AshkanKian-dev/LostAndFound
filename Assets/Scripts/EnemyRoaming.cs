using UnityEngine;

public class EnemyRoaming : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 3f;

    private Vector2 moveDirection;
    private float timer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PickRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionInterval)
        {
            PickRandomDirection();
            timer = 0f;
        }

        // Flip sprite based on direction
        if (moveDirection.x != 0)
            spriteRenderer.flipX = moveDirection.x < 0;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;

    }

    void PickRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        moveDirection = new Vector2(x, y).normalized;
    }
}
