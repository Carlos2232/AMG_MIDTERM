using UnityEngine;

public class EXPGem : MonoBehaviour
{
    public int expValue = 10;
    public float pickupRadius = 1.0f;     
    public float pullRadius = 6.0f;       
    public float pullSpeed = 12f;        

    private Transform player;
    private Rigidbody2D rb;
    private bool isBeingPulled = false;

    void Start()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= pullRadius)
        {
            isBeingPulled = true;

            if (rb != null && rb.simulated)
            {
                rb.simulated = false;
            }
        }

        if (isBeingPulled)
        {
            transform.position = Vector2.Lerp(transform.position, player.position, pullSpeed * Time.deltaTime);
        }

        if (distanceToPlayer < pickupRadius)
        {
            if (ExpManager.Instance != null)
            {
                ExpManager.Instance.AddExp(expValue);
            }
            Destroy(gameObject);
        }
    }

    public void Initialize(int customExpValue)
    {
        expValue = customExpValue;
    }
}