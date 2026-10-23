using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxHP = 20f;
    public float currentHP;
    public float attackDamage = 10f;
    public float attackRadius = 0.8f;
    public int xpValue = 1;
    public GameObject xpGemPrefab;

    [Header("Damage Popups")]
    public GameObject damagePopupPrefab;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    // Knockback 
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.15f;
    private float knockbackSpeed = 8f;
    private Vector3 knockbackDirection;

    // Color caching for hit-flash
    private Color originalColor = Color.white;
    public static Transform CachedPlayer;
    [Header("Custom Hit Detection")]
    public float hitRadius = 1.5f;
    public Vector2 hitOffset = Vector2.zero;
    public bool CheckIfHit(Vector2 attackPosition)
    {
        Vector2 hitCenter = (Vector2)transform.position + hitOffset;
        return Vector2.Distance(transform.position, attackPosition) <= hitRadius;
    }
    protected virtual void OnEnable()
    {
        currentHP = maxHP;

        if (CachedPlayer == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) CachedPlayer = pObj.transform;
        }
        player = CachedPlayer;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public virtual void InitializeStats(float hp, float speed, int wave)
    {
        maxHP = hp;
        currentHP = hp;
        moveSpeed = speed;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        StartCoroutine(FlashRedRoutine());

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDamageSound();
        }

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.8f, -1f);
            GameObject popupObj = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
            popupObj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

            DamagePopup popup = popupObj.GetComponent<DamagePopup>();
            if (popup != null) popup.Setup(damage);
        }

        if (player != null)
        {
            knockbackDirection = (transform.position - player.position).normalized;
            knockbackTimer = knockbackDuration;
        }

        if (currentHP <= 0) Die();
    }

    private IEnumerator FlashRedRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    protected Vector3 GetCurrentVelocity()
    {
        if (player == null) return Vector3.zero;

        Vector3 velocity;

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            velocity = knockbackDirection * knockbackSpeed;
        }
        else
        {
            velocity = (player.position - transform.position).normalized * moveSpeed;
        }

        UpdateAnimations(velocity);

        return velocity;
    }

    protected void UpdateAnimations(Vector3 velocity)
    {
        if (spriteRenderer != null)
        {
            if (velocity.x < -0.05f) spriteRenderer.flipX = true;   
            else if (velocity.x > 0.05f) spriteRenderer.flipX = false; 
        }

        if (animator != null)
        {
            bool isMoving = velocity.sqrMagnitude > 0.01f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    public virtual void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    protected virtual void Die()
    {
        if (xpGemPrefab != null)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0f);
            GameObject gem = Instantiate(xpGemPrefab, spawnPos, Quaternion.identity);

            EXPGem gemScript = gem.GetComponent<EXPGem>();
            if (gemScript != null) gemScript.expValue = xpValue;
        }

        gameObject.SetActive(false);
    }
}