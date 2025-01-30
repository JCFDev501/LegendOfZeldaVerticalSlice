using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoblinStateManager : AnimationStateManager
{

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D
        m_rigidbody = GetComponent<Rigidbody2D>();

        // Get the Animator
        anim = GetComponentInChildren<Animator>();

        m_currentState = MoblinMoveDown;
    }

}
