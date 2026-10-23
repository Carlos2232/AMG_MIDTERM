using UnityEngine;

public class MiniBossEnemy : EnemyBase
{
    [Header("Mini-Boss Attack Pattern")]
    public float detectionRadius = 7f;
    public float postAttackRestDuration = 1f;

    [Header("Prefabs")]
    public GameObject circleBulletPrefab;

    private enum BossState { Chasing, SpreadShooting, Resting, WeaponAttacking }
    private BossState currentState = BossState.Chasing;
    private float stateTimer = 0f;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case BossState.Chasing:
                Vector3 velocity = GetCurrentVelocity();
                transform.Translate(velocity * Time.deltaTime, Space.World);

                if (distanceToPlayer <= detectionRadius)
                {
                    currentState = BossState.SpreadShooting;
                }
                break;

            case BossState.SpreadShooting:
                TriggerAttackAnimation();
                PerformCircularSpreadShot();

                currentState = BossState.Resting;
                stateTimer = postAttackRestDuration;
                break;

            case BossState.Resting:
                stateTimer -= Time.deltaTime;
                UpdateAnimations(Vector3.zero);
                if (stateTimer <= 0f)
                {
                    currentState = BossState.WeaponAttacking;
                }
                break;

            case BossState.WeaponAttacking:
                TriggerAttackAnimation();
                PerformTripleWeaponAttack();

                currentState = BossState.Chasing;
                break;
        }
    }

    void PerformCircularSpreadShot()
    {
        Debug.Log("Titan Core fires Circular Spread Shot!");

        if (circleBulletPrefab != null)
        {
            int bulletCount = 8;
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = i * angleStep;
                Vector3 bulletDir = new Vector3(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                    0f
                );

                GameObject bullet = Instantiate(circleBulletPrefab, transform.position, Quaternion.identity);
                MBBullets bScript = bullet.GetComponent<MBBullets>();
                if (bScript != null)
                {
                    bScript.Initialize(bulletDir);
                }
            }
        }
    }

    void PerformTripleWeaponAttack()
    {
        Debug.Log("Titan Core fires 3x Weapon Attack!");

        if (circleBulletPrefab != null && player != null)
        {
            Vector3 baseDir = (player.position - transform.position).normalized;
            float angleOffset = 20f;

            for (int i = -1; i <= 1; i++)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, i * angleOffset);
                Vector3 spreadDir = rotation * baseDir;

                GameObject bullet = Instantiate(circleBulletPrefab, transform.position, Quaternion.identity);
                MBBullets bScript = bullet.GetComponent<MBBullets>();
                if (bScript != null)
                {
                    bScript.Initialize(spreadDir);
                }
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Titan Core defeated!");
        base.Die();
    }
}