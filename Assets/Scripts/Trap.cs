using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float m_speed = 5;

    private Rigidbody2D m_trapRB;
    private Vector3 m_startPos;
    private bool m_isReversing = false;
    private Vector2 m_currVelocity;

    void Start()
    {
        m_trapRB = GetComponent<Rigidbody2D>();
        m_startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isReversing)
        {
            // Keep track if it went back to the starting position
            Vector3 currPos = m_trapRB.transform.position;
            if(Vector3.Distance(currPos, m_startPos) < 0.1f)
            {
                m_isReversing = false;
                m_trapRB.velocity = new Vector2(0, 0);
            }    
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit something, so reverse velocity
        ReverseVel();
    }

    public void StartMoving(Vector2 velocity)
    {
        m_currVelocity = velocity * m_speed;
        m_trapRB.velocity = m_currVelocity;
    }

    public bool IsMoving()
    {
        if (m_trapRB.velocity != new Vector2() || m_isReversing)
            return true;

        return false;
    }

    private void ReverseVel()
    {
        m_isReversing = true;
        m_trapRB.velocity = -m_currVelocity;
    }
}
