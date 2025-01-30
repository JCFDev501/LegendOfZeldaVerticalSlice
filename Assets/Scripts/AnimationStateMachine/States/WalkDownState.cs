using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkDownState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering WalkDown State");

        // Set the walking down animation to true...
        manager.anim.SetBool("IsWalkingDown", true);

        // Set the last direction to down...
        //manager.anim.SetBool("IsLastDirDown", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.idel)
        {
            // Set the walking down animation to true...
            manager.anim.SetBool("IsWalkingDown", false);
            manager.anim.SetBool("IsLastDirDown", true);

            if (manager.m_playerController.m_state == PlayerStates.movingSouth)
            {
                manager.anim.SetBool("IsWalkingDown", true);
                //manager.anim.SetBool("IsLastDirDown", true);
            }
            else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
                manager.TransitionToState(manager.WalkUp);
            else if (manager.m_playerController.m_state == PlayerStates.movingEast)
                manager.TransitionToState(manager.WalkRight);
            else if (manager.m_playerController.m_state == PlayerStates.movingWest)
                manager.TransitionToState(manager.WalkLeft);
        }

        if (manager.m_playerController.m_state == PlayerStates.movingSouth)
        {
            manager.anim.SetBool("IsWalkingDown", true);
            //manager.anim.SetBool("IsLastDirDown", true);
        }

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.movingEast)
            manager.TransitionToState(manager.WalkRight);
        else if (manager.m_playerController.m_state == PlayerStates.movingWest)
            manager.TransitionToState(manager.WalkLeft);
        else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
            manager.TransitionToState(manager.WalkUp);
        else if (manager.m_playerController.m_state == PlayerStates.attackSouth)
            manager.TransitionToState(manager.AttackDown);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting WalkDown State");
        manager.anim.SetBool("IsWalkingDown", false);
        manager.anim.SetBool("IsLastDirDown", false);

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
