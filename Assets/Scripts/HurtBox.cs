using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private int m_damage = 1;
    [SerializeField] private float m_attackTime = 0.5f;
    void Awake()
    {
        StartCoroutine(AttackTime());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            if (targetHealth != null) 
            {
                targetHealth.UpdateHealth(-m_damage);
            }
        }
    }
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(m_attackTime);
        Destroy(gameObject);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public float GetAttackTime()
    {
        return m_attackTime;
    }
}
