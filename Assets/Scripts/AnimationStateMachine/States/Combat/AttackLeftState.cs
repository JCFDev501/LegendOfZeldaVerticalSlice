using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLeftState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering LeftAttack State");
        manager.anim.SetBool("IsAttackingLeft", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        manager.TransitionToState(manager.WalkLeft);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting LeftAttack State");
        manager.anim.SetBool("IsAttackingLeft", false);
    }
}
