using UnityEngine;
using System.Collections;

public class Slime : Enemy
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stopDistance = 3f;

    [Header("Attack")]
    [SerializeField] int damage = 100;
    [SerializeField] float attackRange = 5f;
    [SerializeField] LayerMask playerLayer;

    [Header("Components")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip castClip;

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

        if (distance > stopDistance && !isAttacking)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
        else if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void Swing()
    {
        StartCoroutine(ResetAttack());

        audioSource.clip = castClip;
        audioSource.Play();

        Vector3 attackPos = transform.position;

        Collider2D hit = Physics2D.OverlapCircle(attackPos, attackRange, playerLayer);

        if (hit != null)
        {
            hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
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
