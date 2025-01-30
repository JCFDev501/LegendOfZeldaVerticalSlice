using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoblinAIController : EnemyAIController
{
    [Header("Moblin Drop")]
    [SerializeField] private bool m_dropBoomerang = false;
    [SerializeField] private GameObject m_boomerang;

    private Vector3 m_attackTargePos;
    private Vector3 m_attackStartPos;
    private Quaternion m_attackReturnRotation;
    private bool m_attackReturning = false;
    private GameObject m_attack;

    protected override void Update()
    {
        base.Update();

        if(m_attack)
        {
            if (!m_attackReturning && Vector3.Distance(m_attack.transform.position, m_attackTargePos) < 0.05)
            {
                // Return to the starting Pos
                m_attackReturning = true;
                m_attack.GetComponent<Projectile>().SetVelocity(m_attack.GetComponent<Projectile>().GetVelocity() * -1f);
            }

            if (m_attackReturning && m_attack.transform.position == m_attackStartPos)
            {
                Destroy(m_attack);
            }
        }
    }

    protected override void ShootProjectile()
    {
        Vector3 spawnPos = new Vector3();
        Quaternion spawnRos = new Quaternion();
        switch (m_direction)
        {
            case Direction.Up:
                spawnPos = transform.position + new Vector3(0, 1.2f, 0);
                m_attackTargePos = spawnPos + new Vector3(0, 3f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 0);
                m_attackReturnRotation = transform.rotation * Quaternion.Euler(0, 0, 180);
                break;

            case Direction.Down:
                spawnPos = transform.position + new Vector3(0, -1.2f, 0);
                m_attackTargePos = spawnPos + new Vector3(0, -3f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 180);
                m_attackReturnRotation = transform.rotation * Quaternion.Euler(0, 0, 0);
                break;

            case Direction.Left:
                spawnPos = transform.position + new Vector3(-1.2f, 0, 0);
                m_attackTargePos = spawnPos + new Vector3(-3f, 0, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 90);
                m_attackReturnRotation = transform.rotation * Quaternion.Euler(0, 0, 270);
                break;

            case Direction.Right:
                spawnPos = transform.position + new Vector3(1.2f, 0, 0);
                m_attackTargePos = spawnPos + new Vector3(3f, 0, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 270);
                m_attackReturnRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                break;

            default:
                return;
        }

        m_attack = Instantiate(m_projectile, spawnPos, spawnRos);
        m_attackStartPos = spawnPos;
        m_attackReturning = false;
    }

    protected override void OnDeath()
    {
        if (m_dropBoomerang)
        {
            GameObject boomerang = Instantiate(m_boomerang, transform.position, new Quaternion());
        }
        else
        {
            base.OnDeath();
        }
    }
}
