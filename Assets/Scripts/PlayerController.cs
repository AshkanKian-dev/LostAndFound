using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    private Rigidbody2D rb;
    private Vector2 movement;    
    private float xAxis;
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
        Flip();
        Move();
        GetInputs();
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveSpeed * xAxis, rb.linearVelocity.y);
    }
    void Flip()
    {
        if (xAxis > 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
}