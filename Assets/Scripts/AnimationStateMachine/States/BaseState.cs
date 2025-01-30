using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all states
public abstract class BaseState
{
    public abstract void EnterState(AnimationStateManager manager);
    public abstract void UpdateState(AnimationStateManager manager);
    public abstract void ExitState(AnimationStateManager manager);
}

