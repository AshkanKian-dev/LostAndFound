using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Load checkpoint position when the game starts
        if (SaveManager.LoadCheckpoint(out Vector3 savedPosition))
        {
            transform.position = savedPosition;
        }
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
