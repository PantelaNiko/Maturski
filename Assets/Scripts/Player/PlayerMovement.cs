using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MOVE_SPEED = 5f;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator animator;
    [SerializeField] private float minX, maxX, minY, maxY;
    private Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        Vector2 moveDir = movement;

        Vector2 newPos = playerRB.position + moveDir.normalized * MOVE_SPEED * Time.fixedDeltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        playerRB.MovePosition(newPos);
    }
}