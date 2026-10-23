using UnityEngine;

public class TankEnemy : EnemyBase
{
    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    void Update()
    {
        if (player == null) return;

        Vector3 velocity = GetCurrentVelocity();
        transform.Translate(velocity * Time.deltaTime, Space.World);

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRadius)
        {
            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;

                TriggerAttackAnimation();

                PlayerController playerScript = player.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(attackDamage);
                }
            }
        }
    }
}