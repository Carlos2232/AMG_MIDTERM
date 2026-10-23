using UnityEngine;

public class MBBullets : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;
    public float lifetime = 5f;
    public float hitRadius = 0.5f; 

    private Vector3 moveDir = Vector3.right;
    private Transform player;

    public void Initialize(Vector3 direction)
    {
        moveDir = direction.normalized;

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < hitRadius)
            {
                PlayerController playerScript = player.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(damage);
                }
                Destroy(gameObject); 
                return;
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}