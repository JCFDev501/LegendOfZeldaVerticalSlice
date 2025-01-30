using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering Idle State");
        manager.anim.SetBool("IsIdle", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        if (manager.m_direction == Vector2.zero)
            return;

        // Transition to the correct walk state...
        if (manager.m_direction is { x: 1, y: 0 })
            manager.TransitionToState(manager.WalkRight);
        else if (manager.m_direction is { x: -1, y: 0 })
            manager.TransitionToState(manager.WalkLeft);
        else if (manager.m_direction is { x: 0, y: 1 })
            manager.TransitionToState(manager.WalkUp);
        else if (manager.m_direction is { x: 0, y: -1 })
            manager.TransitionToState(manager.WalkDown);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting Idle State");
        manager.anim.SetBool("IsIdle", false);
    }

    
}
