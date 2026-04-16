using UnityEngine;

public class MagicOrbSpell : Spell
{
    [Header("Orb Settings")]
    [SerializeField] float speed = 15f;
    [SerializeField] float damage = 50f;
    [SerializeField] float lifetime = 5f;

    private Vector3 direction;

    public override void Cast(Vector3 mouseClickPos)
    {
        direction = (mouseClickPos - transform.position).normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}