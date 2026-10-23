using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 0.8f;
    private Color textColor;
    private Vector3 moveVector;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textColor = textMesh.color;
        }

        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 randomDir = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);

        moveVector = (randomDir * Random.Range(2f, 4f)) + new Vector3(0f, 2f, 0f);
    }

    public void Setup(float damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
        }
    }

    void Update()
    {
        transform.position += moveVector * Time.deltaTime;

        moveVector -= moveVector * 4f * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0f && textMesh != null)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}