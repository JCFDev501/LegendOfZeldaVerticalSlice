using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkUpState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering WalkUp State");

        manager.anim.SetBool("IsWalkingUp", true);

        // Set the last direction to down...
        //manager.anim.SetBool("IsLastDirUp", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.idel)
        {
            // Set the walking down animation to true...
            manager.anim.SetBool("IsWalkingUp", false);
            manager.anim.SetBool("IsLastDirUp", true);

            if (manager.m_playerController.m_state == PlayerStates.movingNorth)
            {
                manager.anim.SetBool("IsWalkingUp", true);
                //manager.anim.SetBool("IsLastDirUp", true);

            }
            else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
                manager.TransitionToState(manager.WalkDown);
            else if (manager.m_playerController.m_state == PlayerStates.movingEast)
                manager.TransitionToState(manager.WalkRight);
            else if (manager.m_playerController.m_state == PlayerStates.movingWest)
                manager.TransitionToState(manager.WalkLeft);
        }

        if (manager.m_playerController.m_state == PlayerStates.movingNorth)
        {
            manager.anim.SetBool("IsWalkingUp", true);
            //manager.anim.SetBool("IsLastDirUp", true);
        }

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.movingEast)
            manager.TransitionToState(manager.WalkRight);
        else if (manager.m_playerController.m_state == PlayerStates.movingWest)
            manager.TransitionToState(manager.WalkLeft);
        else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
            manager.TransitionToState(manager.WalkDown);
        else if (manager.m_playerController.m_state == PlayerStates.attackNorth)
            manager.TransitionToState(manager.AttackUp);

    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting WalkUp State");
        manager.anim.SetBool("IsWalkingUp", false);

        manager.anim.SetBool("IsLastDirUp", false);

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
