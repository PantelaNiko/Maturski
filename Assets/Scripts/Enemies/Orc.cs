using UnityEngine;
using System.Collections;

public class Orc : Enemy
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stopDistance = 1.5f;

    [Header("Attack")]
    [SerializeField] int damage = 10;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] LayerMask playerLayer;

    [Header("Components")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    private Transform playerTransform;
    private bool isAttacking;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        Vector3 dir = playerTransform.position - transform.position;
        float distance = dir.magnitude;

        spriteRenderer.flipX = dir.x < 0;

        if (distance > stopDistance && !isAttacking)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
        else if (!isAttacking)
        {
            animator.SetTrigger("Swing");
            isAttacking = true;
        }
    }

    public void Swing()
    {
        StartCoroutine(ResetAttack());

        Vector3 attackPos = transform.position +
            (spriteRenderer.flipX ? Vector3.left : Vector3.right) * attackRange;

        Collider2D hit = Physics2D.OverlapCircle(attackPos, 1f, playerLayer);

        if (hit != null)
        {
            hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    public override float TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();

        return damage;
    }

    void Die()
    {
        RewardPlayer();
        Destroy(gameObject);
    }
}