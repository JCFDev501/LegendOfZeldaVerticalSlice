using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private GameObject m_acutalTrap;

    private Trap m_trap;

    // Start is called before the first frame update
    void Start()
    {
        m_trap = m_acutalTrap.GetComponent<Trap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't do anything if already in action
        if (m_trap.IsMoving())
            return;

        if (collision.tag == "Player")
        {
            if (name == "Horizontal Trigger")
            {
                // Check if left or right
                float targetPosX = collision.transform.position.x;
                float currPosX = m_trap.transform.position.x;
                float deltaPosX = targetPosX - currPosX;
                if (deltaPosX > 0)
                {
                    // Right
                    m_trap.StartMoving(new Vector2(1, 0));
                }
                else
                {
                    // Left
                    m_trap.StartMoving(new Vector2(-1, 0));
                }
            }
            else // If (name == "Vertical Trigger")
            {
                // Check if up or down
                float targetPosY = collision.transform.position.y;
                float currPosY = m_trap.transform.position.y;
                float deltaPosY = targetPosY - currPosY;
                if (deltaPosY > 0)
                {
                    // Up
                    m_trap.StartMoving(new Vector2(0, 1));
                }
                else
                {
                    // Down
                    m_trap.StartMoving(new Vector2(0, -1));
                }
            }
        }
    }
}
