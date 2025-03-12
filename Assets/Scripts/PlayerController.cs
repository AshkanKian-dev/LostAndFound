using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get Input from WASD or Arrow Keys
        movement.x = Input.GetAxisRaw("Horizontal");  // A (-1) | D (+1)
        movement.y = Input.GetAxisRaw("Vertical");    // S (-1) | W (+1)
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}
