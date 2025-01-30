using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctarockMoveLeftState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingLeft", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        // Transition to the correct walk state...
        if (manager.m_aIController.m_direction == EnemyAIController.Direction.Up)
            manager.TransitionToState(manager.OctarockMoveUp);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Down)
            manager.TransitionToState(manager.OctarockMoveDown);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Right)
            manager.TransitionToState(manager.OctarockMoveRight);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingLeft", false);
    }
}
