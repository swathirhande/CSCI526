using UnityEngine;

public class IceRing : MonoBehaviour
{
    public float fadeSpeed = 1f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = spriteRenderer.color;

        color.a -= fadeSpeed * Time.deltaTime;

        color.a = Mathf.Clamp01(color.a);

        spriteRenderer.color = color;

        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
