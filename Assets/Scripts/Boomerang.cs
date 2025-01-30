using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private Vector2 m_direction;
    private Transform m_player;
    private Rigidbody2D m_rigidbody;
    private bool m_returning = false;
    private PlayerController m_playerController;


    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_returnDelay = 0.5f; // Delay before it starts returning
    [SerializeField] private float m_maxDistance = 5f; // Maximum distance before returning
    [SerializeField] private GameObject m_attachedItem = null;

    private void Start()
    {
        GameObject foundObject = GameObject.Find("Link");
        if (foundObject != null)
        {
            m_playerController = foundObject.GetComponent<PlayerController>();
        }
        else
        {
            // GameObject not found
            Debug.LogError("GameObject not found");
        }
    }

    public void Initialize(int direction, Transform player)
    {
        m_player = player;
        m_rigidbody = GetComponent<Rigidbody2D>();

        switch (direction)
        {
            case 1: // North
                m_direction = Vector2.up;
                break;
            case 2: // South
                m_direction = Vector2.down;
                break;
            case 3: // East
                m_direction = Vector2.right;
                break;
            case 4: // West
                m_direction = Vector2.left;
                break;
        }

        StartCoroutine(ReturnBoomerang());
    }

    private void FixedUpdate()
    {
        if (!m_returning)
        {
            m_rigidbody.velocity = m_direction * m_speed;
        }
        else
        {
            Vector2 directionToPlayer = ((Vector2)m_player.position - (Vector2)transform.position).normalized;
            m_rigidbody.velocity = directionToPlayer * m_speed;

            // Move the attached collectable with the boomerang
            if (m_attachedItem != null)
            {
                m_attachedItem.transform.position = transform.position;
            }
        }
    }

    private IEnumerator ReturnBoomerang()
    {
        yield return new WaitForSeconds(m_returnDelay);

        // Switch to returning mode
        m_returning = true;

        // Ensure the boomerang returns after traveling a certain distance
        yield return new WaitForSeconds(m_maxDistance / m_speed);
        m_returning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_returning && collision.transform == m_player)
        {
            // Notify the player that the boomerang is no longer active
            if (m_playerController != null)
            {
                m_playerController.OnBoomerangDestroyed();
            }
            Destroy(gameObject); // Destroy the boomerang when it returns to the player
        }

        if (collision.tag == "Enemy")
        {
            // Notify the player that the boomerang is no longer active
            if (m_playerController != null)
            {
                m_playerController.OnBoomerangDestroyed();
            }
            Destroy(gameObject); // Destroy the boomerang when it returns to the player
        }

        if (collision.tag == "Collectable")
        {
            // Attach the collectable to the boomerang
            m_attachedItem = collision.gameObject;
            m_attachedItem.transform.parent = transform;
        }
    }

    public float GetAttackTime()
    {
        return m_maxDistance / m_speed + m_returnDelay; // Total time for boomerang to return
    }
}
