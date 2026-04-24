using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] PlayerStats stats;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator animator;
    [SerializeField] private float minX, maxX, minY, maxY;
    private Vector2 movement;

    void Start()
    {
        moveSpeed = stats.GetStat(StatType.Speed);
    }
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

        Vector2 newPos = playerRB.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        playerRB.MovePosition(newPos);
    }
    void OnEnable()
    {
        stats.OnStatChanged += HandleStatChanged;
    }

    void OnDisable()
    {
        stats.OnStatChanged -= HandleStatChanged;
    }

    void HandleStatChanged(StatType type, float value)
    {
        if (type == StatType.Speed)
        {
            moveSpeed = value;
        }
    }
}