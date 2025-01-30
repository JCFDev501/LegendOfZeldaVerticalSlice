using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : Collectable
{
    [SerializeField] private float m_freezeTime = 3f; 
    [SerializeField] private PlayerController m_link; 
    
    public override void UseItem(GameObject user)
    {
        // Enemy Manager
        EnemyManager enemys = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        enemys.FreezeAll(m_freezeTime);

        m_link.m_flashEffect.FlashTheColors(m_freezeTime);

        // Destroy GameObject
        Destroy(gameObject);
    }
}
