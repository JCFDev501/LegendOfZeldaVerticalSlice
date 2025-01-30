using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int m_damage = 1;
    [SerializeField] private float m_attackTime = 0.5f;
    [SerializeField] private float m_attackDuration = 2f;
    [SerializeField] private float m_speed = 0.01f;
    [SerializeField] private float m_stopDuration = 0.1f; // Time to wait before destroying after stopping
    [SerializeField] private GameObject m_AfterEffectPrefab = null;  

    private Rigidbody2D m_rigidbody;
    private bool m_isStopped = false;
    private Vector2 m_velocity;
    public CameraController m_cameraController;

    void Start()
    {
        m_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(AttackTime());

        m_velocity = transform.up * m_speed;
    }

    void Update()
    {
        if (m_cameraController.IsMoving())
            Destroy(gameObject);

        if (!m_isStopped)
        {
            m_rigidbody.velocity = m_velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (this.gameObject.tag == "LinkAttack")
            {
                // Instantiate the explosion effect
                Instantiate(m_AfterEffectPrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }

            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Item")
            {

                //StartCoroutine(StopAndDestroy());
                return; // Ignore this collision
            }

            Health targetHealth = collision.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                //Destroy(gameObject);                      @!!!!!!!!
                //targetHealth.UpdateHealth(-m_damage);
                //StartCoroutine(StopAndDestroy());
            }

            

        }
    }

    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(m_attackDuration);
        StartCoroutine(StopAndDestroy());
    }

    private IEnumerator StopAndDestroy()
    {
        // Stop the projectile
        m_isStopped = true;
        m_rigidbody.velocity = Vector2.zero;

        // Wait for the stop duration
        yield return new WaitForSeconds(m_stopDuration);

        // Destroy the projectile
        Destroy(gameObject);
    }

    public float GetAttackTime()
    {
        return m_attackTime;
    }

    public void SetVelocity(Vector2 set)
    {
        m_velocity = set;
    }

    public Vector2 GetVelocity()
    {
        return m_velocity;
    }
}
