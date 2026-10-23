using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public float fireInterval = 1.0f;
    public float range = 12f;
    public float damage = 15f;
    public float beamDuration = 0.15f;

    private float fireTimer = 0f;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private float showBeamTimer = 0f;
    private bool isFiring = false;

    void Start()
    {
        LineRenderer initialLr = GetComponent<LineRenderer>();
        lineRenderers.Add(initialLr);
        initialLr.positionCount = 2;
        initialLr.enabled = false;
    }

    void Update()
    {
        int currentLvl = LevelManager.Instance != null ? LevelManager.Instance.laserLevel : 1;

        float activeInterval = fireInterval * Mathf.Pow(0.9f, currentLvl);

        fireTimer += Time.deltaTime;
        if (fireTimer >= activeInterval)
        {
            fireTimer = 0f;
            FireLaser(currentLvl);
        }

        if (isFiring)
        {
            showBeamTimer += Time.deltaTime;
            if (showBeamTimer >= beamDuration)
            {
                DisableAllLines();
                isFiring = false;
            }
        }
    }

    void FireLaser(int currentLvl)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        System.Array.Sort(enemies, (a, b) =>
            Vector2.Distance(transform.position, a.transform.position)
            .CompareTo(Vector2.Distance(transform.position, b.transform.position))
        );

        int projectileCount = (currentLvl >= 10) ? enemies.Length : Mathf.Min(currentLvl, enemies.Length);

        while (lineRenderers.Count < projectileCount)
        {
            GameObject newLineObj = new GameObject("LaserBeam_Extra_" + lineRenderers.Count);
            newLineObj.transform.SetParent(transform);
            LineRenderer newLr = newLineObj.AddComponent<LineRenderer>();

            newLr.material = lineRenderers[0].material;
            newLr.startWidth = lineRenderers[0].startWidth;
            newLr.endWidth = lineRenderers[0].endWidth;
            newLr.positionCount = 2;
            newLr.enabled = false;

            lineRenderers.Add(newLr);
        }

        DisableAllLines();

        bool hitAny = false;
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject targetEnemy = enemies[i];
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.transform.position);

            if (distanceToEnemy <= range)
            {
                EnemyBase enemyScript = targetEnemy.GetComponent<EnemyBase>();
                if (enemyScript != null)
                {
                    float finalDamage = (currentLvl >= 10) ? damage * 2.5f : damage;
                    enemyScript.TakeDamage(finalDamage);
                    hitAny = true;
                }

                LineRenderer lr = lineRenderers[i];
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, targetEnemy.transform.position);
                lr.enabled = true;
            }
        }

        if (hitAny && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWeaponSound(0);
        }

        isFiring = true;
        showBeamTimer = 0f;
    }

    void DisableAllLines()
    {
        foreach (LineRenderer lr in lineRenderers)
        {
            if (lr != null) lr.enabled = false;
        }
    }
}