using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRightState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering WalkRight State");

        manager.anim.SetBool("IsWalkingRight", true);

       // manager.anim.SetBool("IsLastDirRight", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.idel)
        {
            // Set the walking down animation to true...
            manager.anim.SetBool("IsWalkingRight", false);
            manager.anim.SetBool("IsLastDirRight", true);

            if (manager.m_playerController.m_state == PlayerStates.movingEast)
            {
                manager.anim.SetBool("IsWalkingRight", true);
                //manager.anim.SetBool("IsLastDirRight", true);
            }
            else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
                manager.TransitionToState(manager.WalkDown);
            else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
                manager.TransitionToState(manager.WalkUp);
            else if (manager.m_playerController.m_state == PlayerStates.movingWest)
                manager.TransitionToState(manager.WalkLeft);
        }

        if (manager.m_playerController.m_state == PlayerStates.movingEast)
        {
            manager.anim.SetBool("IsWalkingRight", true);
            //manager.anim.SetBool("IsLastDirRight", true);
        }

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.movingWest)
            manager.TransitionToState(manager.WalkLeft);
        else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
            manager.TransitionToState(manager.WalkUp);
        else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
            manager.TransitionToState(manager.WalkDown);
        else if (manager.m_playerController.m_state == PlayerStates.attackEast)
            manager.TransitionToState(manager.AttackRight);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting WalkRight State");
        manager.anim.SetBool("IsWalkingRight", false);
        manager.anim.SetBool("IsLastDirRight", false);

       // // Get New last direction...
       // if (manager.m_controller.m_lastDir == 1)
       // {
       //     manager.anim.SetBool("IsLastDirUp", true);
       // }
       // else if (manager.m_controller.m_lastDir == 2)
       // {
       //     manager.anim.SetBool("IsLastDirDown", true);
       // }
       // else if (manager.m_controller.m_lastDir == 3)
       // {
       //     manager.anim.SetBool("IsLastDirRight", true);
       // }
       // else if (manager.m_controller.m_lastDir == 4)
       // {
       //     manager.anim.SetBool("IsLastDirLeft", true);
       // }
    }
}
