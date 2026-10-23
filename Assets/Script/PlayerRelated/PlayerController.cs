using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxHP = 100f;
    public float currentHP;

    [Header("Damage Flash Settings")]
    public float flashDuration = 0.15f;
    private Color originalColor = Color.white;
    private Coroutine flashCoroutine;

    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        ReadInput();
        MovePlayer();
    }

    void ReadInput()
    {
        float x = 0f;
        float y = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        }

        moveInput = new Vector2(x, y).normalized;
    }

    void MovePlayer()
    {
        if (moveInput.sqrMagnitude > 0)
        {
            Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            if (spriteRenderer != null)
            {
                if (moveInput.x < 0) spriteRenderer.flipX = true;
                else if (moveInput.x > 0) spriteRenderer.flipX = false;
            }

            if (animator != null) animator.SetBool("isMoving", true);
        }
        else
        {
            if (animator != null) animator.SetBool("isMoving", false);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        TriggerFlash();

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            Debug.Log("Player Defeated!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }

    public void TriggerFlash()
    {
        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}