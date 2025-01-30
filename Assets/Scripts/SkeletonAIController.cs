using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAIController : EnemyAIController
{
    [Header("Skeleton Drop")]
    [SerializeField] private bool m_dropKey = false;
    [SerializeField] private GameObject m_key;
    
    protected override void OnDeath()
    {
        if(m_dropKey)
        {
            Instantiate(m_key, transform.position, new Quaternion());
        }
        else
        {
            base.OnDeath();
        }
    }
}
