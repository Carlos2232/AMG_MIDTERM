using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    public GameObject bulletPrefab;
    public float stoppingDistance = 5f;
    public float fireRate = 2f;
    private float fireTimer = 0f;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 velocity = GetCurrentVelocity();
            transform.Translate(velocity * Time.deltaTime, Space.World);
        }
        else
        {
            Vector3 velocity = GetCurrentVelocity();

            
            if (velocity.magnitude > moveSpeed) 
            {
                transform.Translate(velocity * Time.deltaTime, Space.World);
            }
            else
            {
                UpdateAnimations(Vector3.zero); 
            }
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            TriggerAttackAnimation();

            Vector3 direction = (player.position - transform.position).normalized;
            Shoot(direction);
        }
    }

    void Shoot(Vector3 dir)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(dir);
            }
        }
    }
}