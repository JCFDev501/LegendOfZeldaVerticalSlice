using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDownState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering DownAttack State");
        manager.anim.SetBool("IsAttackingDown", true);

        
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        manager.TransitionToState(manager.WalkDown);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting DownAttack State");
        manager.anim.SetBool("IsAttackingDown", false);
    }
}
