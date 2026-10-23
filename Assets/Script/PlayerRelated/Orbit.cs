using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject orbPrefab;
    public float orbitSpeed = 150f;
    public float orbitRadius = 2.5f;
    public float baseDamage = 10f;

    [Header("Blade Spin Settings")]
    public float bladeRotationSpeed = 400f;

    [Header("Weapon Traits / Modifiers")]
    public bool hasSharpEdges = false;
    public bool hasWideOrbit = false;
    public bool hasOverclock = false;

    private float currentAngle = 0f;
    private List<GameObject> activeOrbs = new List<GameObject>();
    private int lastOrbCount = 0;

    private Dictionary<EnemyBase, float> hitCooldowns = new Dictionary<EnemyBase, float>();

    void Update()
    {
        int currentLvl = LevelManager.Instance != null ? LevelManager.Instance.orbitLevel : 1;
        int targetOrbCount = currentLvl;

        if (targetOrbCount != lastOrbCount)
        {
            UpdateOrbVisuals(targetOrbCount);
            lastOrbCount = targetOrbCount;
        }

        float speedMultiplier = hasOverclock ? 1.5f : 1.0f;
        float activeSpeed = (orbitSpeed + (currentLvl * 10f)) * speedMultiplier;
        currentAngle += activeSpeed * Time.deltaTime;

        if (activeOrbs.Count == 0) return;

        float angleStep = 360f / activeOrbs.Count;

        float damageMultiplier = hasSharpEdges ? 1.5f : 1.0f;
        float activeDamage = (baseDamage + (currentLvl * 3f)) * damageMultiplier;

        float radiusMultiplier = hasWideOrbit ? 1.35f : 1.0f;
        float currentRadius = orbitRadius * radiusMultiplier;

        List<EnemyBase> keysToRemove = new List<EnemyBase>();
        foreach (KeyValuePair<EnemyBase, float> kvp in hitCooldowns)
        {
            if (kvp.Key == null || !kvp.Key.gameObject.activeInHierarchy)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        foreach (var key in keysToRemove) { hitCooldowns.Remove(key); }

        for (int i = 0; i < activeOrbs.Count; i++)
        {
            if (activeOrbs[i] == null) continue;

            float angle = currentAngle + (i * angleStep);
            float rad = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * currentRadius;
            float y = Mathf.Sin(rad) * currentRadius;

            Vector3 orbPosition = transform.position + new Vector3(x, y, 0f);
            activeOrbs[i].transform.position = orbPosition;

            float currentBladeSpin = bladeRotationSpeed * speedMultiplier;
            activeOrbs[i].transform.Rotate(0f, 0f, currentBladeSpin * Time.deltaTime);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;

                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                if (enemyScript != null)
                {
                    if (enemyScript.CheckIfHit(orbPosition))
                    {
                        float lastHitTime = hitCooldowns.ContainsKey(enemyScript) ? hitCooldowns[enemyScript] : 0f;

                        if (Time.time >= lastHitTime + 0.2f)
                        {
                            hitCooldowns[enemyScript] = Time.time;
                            enemyScript.TakeDamage(activeDamage);

                            if (AudioManager.Instance != null)
                            {
                                AudioManager.Instance.PlayWeaponSound(1);
                            }
                        }
                    }
                }
            }
        }
    }

    void UpdateOrbVisuals(int count)
    {
        foreach (GameObject orb in activeOrbs)
        {
            if (orb != null) Destroy(orb);
        }
        activeOrbs.Clear();

        if (orbPrefab != null)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newOrb = Instantiate(orbPrefab, transform.position, Quaternion.identity, transform);
                activeOrbs.Add(newOrb);
            }
        }
    }
}