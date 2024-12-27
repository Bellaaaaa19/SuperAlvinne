using UnityEngine;

public class InvisibleWallBehavior : MonoBehaviour
{
    public float fadeDuration = 10f;  // Duration to fade in (10 seconds)
    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private Color endColor;

    void Start()
    {
        // Get the SpriteRenderer component attached to the wall
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the starting color to fully transparent
        startColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        // Set the target color to fully opaque
        endColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // Set the wall initially invisible
        spriteRenderer.color = startColor;

        // Start the fade-in effect after a delay
        StartCoroutine(FadeInAfterDelay());
    }

    // Coroutine to wait 10 seconds before starting to fade in
    private System.Collections.IEnumerator FadeInAfterDelay()
    {
        // Wait for 10 seconds
        yield return new WaitForSeconds(10f);

        // Fade in over time
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;  // Wait until the next frame
        }

        // Ensure the final color is fully opaque
        spriteRenderer.color = endColor;
    }

    // This method will stop the 2D image if it collides with the wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Stop the 2D image's movement
                rb.linearVelocity = Vector2.zero; // You can also use rb.velocity = new Vector2(0, 0);
            }
        }
    }
}
