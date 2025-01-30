using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctarockMoveUpState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingUp", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        // Transition to the correct walk state...
        if (manager.m_aIController.m_direction == EnemyAIController.Direction.Right)
            manager.TransitionToState(manager.OctarockMoveRight);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Down)
            manager.TransitionToState(manager.OctarockMoveDown);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Left)
            manager.TransitionToState(manager.OctarockMoveLeft);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingUp", false);
    }
}
