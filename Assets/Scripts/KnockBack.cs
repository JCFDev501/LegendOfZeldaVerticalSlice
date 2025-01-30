using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float KnockbackStrength = 3f;
    [SerializeField] public bool isKnockbackActive = false;
    private Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 knockbackDirection)
    {
        float initialSpeedMultiplier = 2.0f; // Adjust this to increase speed
        float knockbackTime = 0.15f; // Adjust this to decrease duration

        m_rigidbody.velocity = knockbackDirection * KnockbackStrength * initialSpeedMultiplier;
        isKnockbackActive = true;
        StartCoroutine(ResetKnockback(knockbackTime));
    }

    private IEnumerator ResetKnockback(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_rigidbody.velocity = Vector2.zero;
        isKnockbackActive = false;
    }
}
