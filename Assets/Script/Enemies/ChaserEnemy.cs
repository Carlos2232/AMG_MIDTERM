using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    private float attackCooldown = 1f; 
    private float attackTimer = 0f;

    void Update()
    {
        if (player == null) return;

        transform.position += GetCurrentVelocity() * Time.deltaTime;

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