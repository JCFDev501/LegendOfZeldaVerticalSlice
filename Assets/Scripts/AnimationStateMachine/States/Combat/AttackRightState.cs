using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRightState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering RightAttack State");
        manager.anim.SetBool("IsAttackingRight", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        manager.TransitionToState(manager.WalkRight);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting RightAttack State");
        manager.anim.SetBool("IsAttackingRight", false);
    }
}
