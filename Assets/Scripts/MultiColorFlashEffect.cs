using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiColorFlashEffect : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer; // The sprite renderer to flash
    public List<Color> FlashColors; // The colors to flash
    public float FlashDuration = 0.1f; // The duration of each flash
    public bool Looping = false; // Whether to keep flashing the colors

    private Color originalColor;

    private void Start()
    {
        // If the sprite renderer is not set, try to get it from the game object
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color
        originalColor = SpriteRenderer.color;

        if (Looping)
        {
            FlashTheColors();
        }
    }

    public void FlashTheColors(float duration = -1)
    {
        StartCoroutine(FlashColorsRoutine(duration)); // Start the coroutine
    }

    private IEnumerator FlashColorsRoutine(float duration)
    {
        float elapsedTime = 0f;

        while ((Looping || elapsedTime < duration) && FlashColors.Count > 0)
        {
            // Loop through each color and flash it
            foreach (var color in FlashColors)
            {
                SpriteRenderer.color = color;
                yield return new WaitForSeconds(FlashDuration);

                if (!Looping)
                {
                    elapsedTime += FlashDuration;
                    if (elapsedTime >= duration)
                        break;
                }
            }

            // Reset the color to the original color
            SpriteRenderer.color = originalColor;

            if (!Looping && elapsedTime >= duration)
                break;
        }
    }
}
