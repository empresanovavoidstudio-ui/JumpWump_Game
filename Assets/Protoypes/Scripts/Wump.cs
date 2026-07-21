using UnityEngine;

public class Wump : MonoBehaviour
{
    public float jump = 5f;
    public float speed = 7f;
    Rigidbody2D physicsPlayer;
    Vector2 move;
    void Start()
    {
        physicsPlayer = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        move = new Vector2(moveX, moveY).normalized;
        physicsPlayer.linearVelocity = new Vector2(move.x * speed, physicsPlayer.linearVelocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }
    }
    void JumpPlayer()
    {
        physicsPlayer.linearVelocity = new Vector2(physicsPlayer.linearVelocity.x, jump);
    }
}