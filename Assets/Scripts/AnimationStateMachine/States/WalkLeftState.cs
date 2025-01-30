using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkLeftState : BaseState
{
    public override void EnterState(AnimationStateManager manager)
    {
        Debug.Log("Entering WalkLeft State");

        manager.anim.SetBool("IsWalkingLeft", true);

        // Set the last direction to down...
        //manager.anim.SetBool("IsLastDirLeft", true);
    }

    public override void UpdateState(AnimationStateManager manager)
    {
        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.idel)
        {
            // Set the walking down animation to true...
            manager.anim.SetBool("IsWalkingLeft", false);
            manager.anim.SetBool("IsLastDirLeft", true);

            if (manager.m_playerController.m_state == PlayerStates.movingWest)
            {
                manager.anim.SetBool("IsWalkingLeft", true);
                //manager.anim.SetBool("IsLastDirLeft", true);
            }
            else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
                manager.TransitionToState(manager.WalkDown);
            else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
                manager.TransitionToState(manager.WalkUp);
            else if (manager.m_playerController.m_state == PlayerStates.movingEast)
                manager.TransitionToState(manager.WalkRight);
        }

        if (manager.m_playerController.m_state == PlayerStates.movingWest)
        {
            manager.anim.SetBool("IsWalkingLeft", true);
            //manager.anim.SetBool("IsLastDirLeft", true);
        }

        // Transition to the correct walk state...
        if (manager.m_playerController.m_state == PlayerStates.movingEast)
            manager.TransitionToState(manager.WalkRight);
        else if (manager.m_playerController.m_state == PlayerStates.movingNorth)
            manager.TransitionToState(manager.WalkUp);
        else if (manager.m_playerController.m_state == PlayerStates.movingSouth)
            manager.TransitionToState(manager.WalkDown);
        else if (manager.m_playerController.m_state == PlayerStates.attackWest)
            manager.TransitionToState(manager.AttackLeft);
    }

    public override void ExitState(AnimationStateManager manager)
    {
        Debug.Log("Exiting WalkLeft State");
        manager.anim.SetBool("IsWalkingLeft", false);
        manager.anim.SetBool("IsLastDirLeft", false);

       //// Get New last direction...
       //if (manager.m_controller.m_lastDir == 1)
       //{
       //    manager.anim.SetBool("IsLastDirUp", true);
       //}
       //else if (manager.m_controller.m_lastDir == 2)
       //{
       //    manager.anim.SetBool("IsLastDirDown", true);
       //}
       //else if (manager.m_controller.m_lastDir == 3)
       //{
       //    manager.anim.SetBool("IsLastDirRight", true);
       //}
       //else if (manager.m_controller.m_lastDir == 4)
       //{
       //    manager.anim.SetBool("IsLastDirLeft", true);
       //}
    }
}
