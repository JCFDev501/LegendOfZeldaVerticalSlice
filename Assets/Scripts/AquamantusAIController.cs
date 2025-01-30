using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamantusAIController : EnemyAIController
{
    [Header("Player To Attack")]
    [SerializeField] private GameObject m_player;

    [Header("Aquamantus Drop")]
    [SerializeField] private GameObject m_heartContainer;

    private bool m_firstEncounter = true;

    // Start is called before the first frame update
    protected override void ShootProjectile()
    {
        if (m_player == null)
            return;

        // Calculate rotation
        Vector3 playerPos = m_player.transform.position;
        Vector3 deltaPos = playerPos - transform.position;
        Quaternion spawnRos;
        if (deltaPos.x < 0)
            spawnRos = Quaternion.LookRotation(deltaPos) * Quaternion.Euler(0, 0, 90);
        else
            spawnRos = Quaternion.LookRotation(deltaPos) * Quaternion.Euler(0, 0, 270);

        spawnRos.y = 0;
        spawnRos.x = 0;

        Vector3 spawnPos = transform.position;
        for (int i = -1; i <= 1; i++)
        {
            Instantiate(m_projectile, spawnPos, spawnRos * Quaternion.Euler(0, 0, i * 45));
        }
    }

    protected override void ChooseRandomDirection()
    {
        // Get Current Position
        Vector3 currPos = transform.position;

        // Get Random Direction
        int direction = Random.Range(0, 2);
        switch (direction)
        {
            case 0:
                m_direction = Direction.Left;
                m_targetPos = new Vector3(currPos.x - 1f, currPos.y);
                break;

            case 1:
                m_direction = Direction.Right;
                m_targetPos = new Vector3(currPos.x + 1f, currPos.y);
                break;

            default:
                m_direction = Direction.None;
                break;
        }
    }

    protected override void Update()
    {
        base.Update();

        // If player entered the first time
        if (m_inCamera && m_firstEncounter)
        {
            ShootProjectile();
            m_firstEncounter = false;
        }
    }

    protected override void OnDeath()
    {
        Instantiate(m_heartContainer, transform.position, new Quaternion());
    }
}
