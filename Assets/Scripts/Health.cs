using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int m_health = 6;
    [SerializeField] private int m_healthMax = 6;
    [SerializeField] private float m_deathScreenTriggerTime = 4;
    [SerializeField] private GameObject m_DeathStarPrefab;

    public int GetCurrentHealth()
    {
        return m_health;
    }
    public int GetMaxHealth()
    {
        return m_healthMax;
    }
    public void  UpdateHealth(int healthChange)
    {
        if (m_health + healthChange > m_healthMax) { }
            m_health += healthChange;
         
        if (m_health > m_healthMax)
            m_health = m_healthMax;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        m_healthMax = newMaxHealth;
    }

    public void LinksDeath()
    {
        // Color for the game object
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color newColor = spriteRenderer.color;

        // Change the color so the player can't see
        newColor.a = 0;
        spriteRenderer.color = newColor;

        Instantiate(m_DeathStarPrefab, transform.position, Quaternion.identity);

        StartCoroutine(TriggerDeathScreen(m_deathScreenTriggerTime));
    }

    private IEnumerator TriggerDeathScreen(float time)
    {
        yield return new WaitForSeconds(time);

        GetComponent<PlayerController>().SetIsDead(true);
    }
}
