using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoryiaDownState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingDown", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        // Transition to the correct walk state...
        if (manager.m_aIController.m_direction == EnemyAIController.Direction.Up)
            manager.TransitionToState(manager.GoryiaMoveUp);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Left)
            manager.TransitionToState(manager.GoryiaMoveLeft);
        else if (manager.m_aIController.m_direction == EnemyAIController.Direction.Right)
            manager.TransitionToState(manager.GoryiaMoveRight);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        manager.anim.SetBool("MovingDown", false);
    }
}
