using UnityEngine;

public class ShockwavePulse : MonoBehaviour
{
    public float pulseInterval = 3.0f;
    public float pulseRadius = 5.0f;
    public float pulseDamage = 25f;
    public GameObject pulseVisualPrefab;

    private float pulseTimer = 0f;

    void Update()
    {
        pulseTimer += Time.deltaTime;
        if (pulseTimer >= pulseInterval)
        {
            pulseTimer = 0f;
            TriggerPulse();
        }
    }

    void TriggerPulse()
    {
        int currentLvl = LevelManager.Instance != null ? LevelManager.Instance.pulseLevel : 1;

        float activeRadius = pulseRadius + (currentLvl * 0.5f);
        float activeDamage = pulseDamage + (currentLvl * 5f);

        if (pulseVisualPrefab != null)
        {
            GameObject pulseVisual = Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);
            pulseVisual.transform.localScale = Vector3.one * (activeRadius * 2f);
            Destroy(pulseVisual, 0.4f);
        }

        bool hitAny = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= activeRadius)
            {
                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(activeDamage);
                    hitAny = true;

                    if (currentLvl >= 10)
                    {
                        enemyScript.TakeDamage(activeDamage * 0.5f);
                    }
                }
            }
        }

        if (hitAny && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWeaponSound(2);
        }
    }
}