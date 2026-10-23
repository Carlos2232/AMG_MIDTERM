using UnityEngine;

public class CrawlerEnemy : EnemyBase
{
    public float erraticFrequency = 5f;
    public float erraticAmplitude = 2f;

    private float attackCooldown = 1f;
    private float attackTimer = 0f;

    void Update()
    {
        if (player == null) return;

        Vector3 velocity = GetCurrentVelocity();
        Vector3 standardChaseDir = (player.position - transform.position).normalized;

        if (Vector3.Distance(velocity, standardChaseDir * moveSpeed) < 0.01f)
        {
            Vector3 perpendicular = new Vector3(-standardChaseDir.y, standardChaseDir.x, 0f);
            float waveOffset = Mathf.Sin(Time.time * erraticFrequency) * erraticAmplitude;
            Vector3 finalMoveDir = (standardChaseDir + (perpendicular * waveOffset * 0.2f)).normalized;
            velocity = finalMoveDir * moveSpeed;

            UpdateAnimations(velocity);
        }

        transform.Translate(velocity * Time.deltaTime, Space.World);

        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

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