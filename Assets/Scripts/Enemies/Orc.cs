using UnityEngine;
using System.Collections;
public class OrcAttack2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stopDistance = 1.5f;
    [Header("Attack")]
    [SerializeField] int damage = 10;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    private GameObject player;
    private Transform playerTransform;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
    }
    void Update()
    {
        if (player == null) return;

        Vector3 dir = (playerTransform.position - transform.position);
        float distance = dir.magnitude;
        spriteRenderer.flipX = dir.x < 0;
        float moveDistance = moveSpeed * Time.deltaTime;

        if (distance > stopDistance && !isAttacking)
        {
            Vector3 move = dir.normalized * moveDistance;
            transform.position += move;
        }
        else
        {
            if (!isAttacking)
            {
                animator.SetTrigger("Swing");
                isAttacking = true;
            }
        }
    }
    public void Swing()
    {
        StartCoroutine(ResetAttack());
        Vector3 attackPos = transform.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right) * attackRange;

        Collider2D hit = Physics2D.OverlapCircle(attackPos, 2f, playerLayer);
        if (hit != null)
        {
            hit.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    // At the end of your swing (e.g., in Swing() or animation event)

// Inline coroutine
    private IEnumerator ResetAttack()
    {
        // Get info about the current animation on layer 0
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait for the length of the animation
        yield return new WaitForSeconds(stateInfo.length);

        isAttacking = false;
    }
}