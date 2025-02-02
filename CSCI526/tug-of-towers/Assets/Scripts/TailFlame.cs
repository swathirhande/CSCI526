using UnityEngine;

public class TailFlame: MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] flameSprites;

    [Header("Attribute")]
    [SerializeField] private float frameDuration = 0.1f;

    private int currentFrame = 0;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameDuration)
        {
            timer = 0f;

            currentFrame = (currentFrame + 1) % flameSprites.Length;
            spriteRenderer.sprite = flameSprites[currentFrame];
        }
    }
}
