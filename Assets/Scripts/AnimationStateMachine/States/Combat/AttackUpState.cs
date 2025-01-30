using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering UpAttack State");
        manager.anim.SetBool("IsAttackingUp", true);


    }

    public override void UpdateState(AnimationStateManager manager)
    {
        manager.TransitionToState(manager.WalkUp);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting UpAttack State");
        manager.anim.SetBool("IsAttackingUp", false);
    }
}
